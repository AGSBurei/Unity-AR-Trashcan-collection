#include "EspMQTTClient.h"

const String topic = "STC/sensor";
const char* ssid = "Livebox-D280";
const char* password = "22061995";
const String serialNumber = "16543465453";
const int sensorType = 1; // 1 = Tilt | 2 = Photo | 3 = IMU

bool state = false;

EspMQTTClient client(
  ssid,
  password,
  "51.75.121.250",  // MQTT Broker server ip
  "",   // Username Can be omitted if not needed
  "",   // Password Can be omitted if not needed
  "ESP01",      // Client name that uniquely identify your device
  8883
);
 
void setup() 
{
  Serial.begin(115200);
  Serial.println();
  Serial.println("====Projet DEV - Setup====");
}
 
void loop() 
{
  Serial.print(".");
  
  client.loop();

  if(stateChanged()){
    state = !state;
    publishMeasure();
  }

  delay(2000); 
}

void publishMeasure() {
  if(client.isConnected()){
    String strState = state ? "1" : "0";    
    client.publish(topic, "{\"value\":" + strState + ",\"sn\":\"" + serialNumber + "\"}");
  }
}

bool stateChanged(){
  switch(sensorType){
    case 1:
      return readTiltSensor() != state;
      break;
    case 2:
      return readPhotoSensor() != state;
      break;      
    case 3:
      return readIMU() != state;
      break;
  }
}

bool readTiltSensor(){
  //TODO
  return true;
}

bool readPhotoSensor(){
  //TODO
  return true;
}

bool readIMU(){
  //TODO
  return true;
}

void onConnectionEstablished(){
  // DON'T REMOVE
}
