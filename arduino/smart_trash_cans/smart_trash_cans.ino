#include "EspMQTTClient.h"

// Globals
const String serialNumber = "16543465453";
const int sensorType = 1; // 1 = Tilt | 2 = Photo | 3 = IMU
const int ledPin = 16;

// TILT : GND = entrée | D3 = Sortie
const int sensorPin = 4;

// PHOTO RESISTOR : 3v3 = entrée PR | GND = 10kO | A0 = Sortie avec resistance
const int ldrPin = A0;
int limitPhotoResistor = 600;

// IMU : D1 = SLC | D2 = SDA | 3V3 = VCC | GND = GND

// WIFI
const char* ssid = "AP_Thomas";
const char* password = "122345678";

// MQTT
const String publishTopic = "STC/sensor";
const String subscribeTopic = "STC/" + serialNumber + "/isfull";
EspMQTTClient client(
  ssid,
  password,
  "51.75.121.250",  // MQTT Broker server ip
  "",   // Username Can be omitted if not needed
  "",   // Password Can be omitted if not needed
  "ESP01",      // Client name that uniquely identify your device
  8883
);

bool trashCanIsFull = false;
 
void setup() 
{
  Serial.begin(115200);
  Serial.println();
  Serial.println("====Projet DEV - Setup====");

  connectWifi();

  subscribeTopics();

  initLed();
  initSensor();

  // publishMeasure();

}
 
void loop() 
{
  client.loop();

  checkSensor();

  checkIsFull();

  delay(500); 
}

void checkIsFull(){
  
  if(trashCanIsFull){
    // Serial.println("Allume la led");
    digitalWrite(ledPin, HIGH);    
  }else{
    // Serial.println("Eteint la led");
    digitalWrite(ledPin, LOW);    
  }
}

void initLed(){
  pinMode(ledPin, OUTPUT);
}

void connectWifi(){

  Serial.println("Connection au broker");

  while(!client.isConnected()){
    Serial.print(".");
    client.loop();

    delay(500);
  }

  Serial.println("");
  Serial.println("Connecté au broker !");

}

bool subscribeTopics(){

  if(client.isConnected()){

    client.subscribe(subscribeTopic, [] (const String &payload)  {
      Serial.print("Message received with topic '" + subscribeTopic + "' : " );
      Serial.println(payload);

      if(payload == "0"){
        trashCanIsFull = false;
      }

    });

    Serial.println("Le subscribe a bien été envoyée !");
    return true;
  }else{
    Serial.println("La poubelle n'est pas connectée au WIFI");
    return false;
  }

}

void checkSensor(){
  switch(sensorType){
    case 1:
      checkTilt();     
      break;
    case 2:
      checkPhotoResistor();
      break;      
    case 3:
      checkIMU();      
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


// ===== TILT =====

void initTilt(){
  pinMode(sensorPin, INPUT);
}

void checkTilt(){
  
  if(!digitalRead(sensorPin))
  {

    Serial.println("La poubelle est ouverte");

    while(!digitalRead(sensorPin)){
      delay(500);
    }
    
    Serial.println("La poubelle est fermée");       

    trashCanIsFull = !trashCanIsFull;
    publishMeasure(); 
    Serial.print("Etat de la poubelle : ");
    Serial.println(trashCanIsFull);
  }  

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
  
  // Serial.print("Etat de la poubelle : ");
  // Serial.println(trashCanIsFull ? "Pleine" : "Vide");
}

bool photoResistorIsOpen(){
  return analogRead(ldrPin) >= limitPhotoResistor ? true : false; 
}

// ===== IMU =====

void initIMU(){
  // TODO  
}

void checkIMU(){
  // TODO
}

// ===== MQTT =====

bool publishMeasure() {
  if(client.isConnected()){
    String strState = trashCanIsFull ? "1" : "0";    
    client.publish(publishTopic, "{\"value\":" + strState + ",\"sn\":\"" + serialNumber + "\"}");

    Serial.println("L'etat de la poubelle a bien été envoyée !");
    return true;
  }else{
    Serial.println("La poubelle n'est pas connectée au WIFI");
    return false;
  }
}

void onConnectionEstablished(){
  // DON'T REMOVE
}
