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

        // Public Functions

        public void PublishTrashCansEmpty(String trashCanId){
            Publish("STC/" + trashCanId, "{\"sn\":\"" + trashCanId + "\",\"value\":\"0\"}");
        }

        private void SubscribeTrashCan(){
            Subscribe("STC/#");
        }

        // Main Functions

        protected override void Start()
        {
            base.Start();

            //todo need test
            // this.Connect();
        }

        protected override void Update()
        {
            base.Update(); 

            // if (eventMessages.Count > 0)
            // {
            //     foreach (string msg in eventMessages)
            //     {
            //         ProcessMessage(msg);
            //     }
            //     eventMessages.Clear();
            // }
            
        }


        protected override void SubscribeTopics(){
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

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);

            //TODO mettre a jour l'etat de nos poubelle
            Debug.Log("TRASH CAN LIST COUNT");
            Debug.Log(TrashCanManager.trashCanList.Count);

            myTrashCanManager = FindObjectOfType<TrashCanManager>();
            Debug.Log(myTrashCanManager.GetTrashNumber());
        }

        private void Publish(String topic, String body){
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(body), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
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