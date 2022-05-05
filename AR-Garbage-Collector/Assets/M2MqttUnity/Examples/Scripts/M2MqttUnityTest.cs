using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Examples
{
    public class M2MqttUnityTest : M2MqttUnityClient
    {
        private List<string> eventMessages = new List<string>();

        TrashCanManager myTrashCanManager;

        String[,] copyListTrashCan = new String[3, 2];


        // Public Functions

        public void PublishTrashCansEmpty(String trashCanId)
        {
            Debug.Log("HEEEEEEEEEEEEEEEEEEEERE");
            Debug.Log(trashCanId);
            // Publish2("STC/" + trashCanId, "{\"sn\":\"" + trashCanId + "\",\"value\":\"0\"}");
            Publish2("STC/" + trashCanId, "{\"value\":\"0\",\"sn\":\"" + trashCanId + "\"}");
        }

        private void SubscribeTrashCan()
        {
            Subscribe("STC/#");
        }

        // Main Functions

        protected override async void Update()
        {
            base.Update();

            for (int i = 0; i < myTrashCanManager.trashCanList.Count; i++)
            {
                for (int y = 0; y < myTrashCanManager.trashCanList.Count; y++)
                {
                    if (copyListTrashCan[y, 0] == myTrashCanManager.trashCanList[i].serial)
                    {

                        if (copyListTrashCan[y, 1] != myTrashCanManager.trashCanList[i]._isFull.ToString())
                        {
                            if (myTrashCanManager.trashCanList[i]._isFull)
                            {
                                copyListTrashCan[y, 1] = myTrashCanManager.trashCanList[i]._isFull.ToString();
                            }
                            else
                            {
                                string serial = copyListTrashCan[y, 0];
                                Debug.Log(serial);
                                PublishTrashCansEmpty(serial);
                                copyListTrashCan[y, 1] = myTrashCanManager.trashCanList[i]._isFull.ToString();
                            }
                        }
                    }
                }
            }


        }
        protected override async void Start()
        {
            base.Start();

            Debug.Log("START !!!");

            myTrashCanManager = FindObjectOfType<TrashCanManager>();

            for (int i = 0; i < myTrashCanManager.trashCanList.Count; i++)
            {
                copyListTrashCan[i, 0] = myTrashCanManager.trashCanList[i].serial;
                copyListTrashCan[i, 1] = myTrashCanManager.trashCanList[i]._isFull.ToString();
            }

        }


        protected override void SubscribeTopics()
        {
            Debug.Log("SUbscribeTopics Thomas !");

            // TODO a tester
            SubscribeTrashCan();
        }

        // Private Functions

        protected override void OnConnected()
        {
            base.OnConnected();
            Debug.Log("Connected to broker on " + brokerAddress + "\n");

        }

        protected override async void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);

            string sn = msg.Split('\"')[7].Split('\"')[0];
            string value = msg.Split('\"')[3].Split('\"')[0];


            for (int i = 0; i < myTrashCanManager.trashCanList.Count; i++)
            {
                if (sn == myTrashCanManager.trashCanList[i].serial)
                {
                    myTrashCanManager.trashCanList[i]._isFull = value == "0" ? false : true;
                }
            }


        }

        private void Publish2(String topic, String body)
        {
            if (client != null)
            {

                client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(body), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
            else
            {
                Debug.Log("client is null");
            }
        }

        private void Subscribe(String topic)
        {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            Debug.Log("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }

        // private void ProcessMessage(string msg)
        // {
        //     Debug.Log("Received: " + msg);
        // }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            autoConnect = true;
        }
    }
}