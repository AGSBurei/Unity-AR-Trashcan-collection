#include "EspMQTTClient.h"

// PHOTO RESISTOR : 3v3 = entrée PR | GND = 10kO | A0 = Sortie avec resistance
const int ldrPin = A0;

// IMU : D1 = SLC | D2 = SDA | 3V3 = VCC | GND = GND


const String topic = "STC/sensor";
const char* ssid = "AP_Thomas";
const char* password = "122345678";

const String serialNumber = "16543465453";
const int sensorType = 2; // 1 = Tilt | 2 = Photo | 3 = IMU

bool trashCanIsFull = false;

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

  //publishMeasure();
  
  initSensor();
}
 
void loop() 
{
  Serial.print(".");
  client.loop();

  checkSensor();

  delay(1000); 
}

void checkSensor(){
  switch(sensorType){
    case 1:
      // TODO      
      break;
    case 2:
      checkPhotoResistor();
      break;      
    case 3:
      // TODO      
      break;
  }
}

void initSensor(){
    switch(sensorType){
    case 1:
      initTilt();
      break;
    case 2:
      initPhotoResistor();
      break;      
    case 3:
      initIMU();
      break;
  }
}

void initTilt(){
  // TODO  
}

void initIMU(){
  // TODO  
}

// ===== PHOTO RESISTOR =====

void initPhotoResistor(){
  pinMode(ldrPin, INPUT);  
}

void checkPhotoResistor(){

  if(photoResistorIsOpen()){
    Serial.println("La poubelle est ouverte");

    while(photoResistorIsOpen()){
      delay(500);
    }

    Serial.println("La poubelle est refermée");
    trashCanIsFull = !trashCanIsFull;
    publishMeasure();    
  }
  
  Serial.print("Etat de la poubelle : ");
  Serial.println(trashCanIsFull);
}

bool photoResistorIsOpen(){
  return analogRead(ldrPin) >= 500 ? true : false; 
}

// ===== MQTT =====

void publishMeasure() {
  if(client.isConnected()){
    String strState = trashCanIsFull ? "1" : "0";    
    client.publish(topic, "{\"value\":" + strState + ",\"sn\":\"" + serialNumber + "\"}");
  }
}

void onConnectionEstablished(){
  // DON'T REMOVE
}
