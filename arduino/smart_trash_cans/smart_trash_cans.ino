#include "EspMQTTClient.h"
#include "Wire.h" // This library allows you to communicate with I2C devices.

// rx = 7 | io0 = 5 | io2 = 3 | tx = 2

// Globals
// const char* ESPSerialNumber = "16543465453"; const int sensorType = 1; // Tilt
// const char* ESPSerialNumber = "43546546312"; const int sensorType = 2; // Photo
const char* ESPSerialNumber = "97897865"; const int sensorType = 3; // IMU

int ledPin;

// TILT : GND = entrée | D3 = Sortie
const int sensorPin = 4;

// PHOTO RESISTOR : 3v3 = entrée PR | GND = 10kO | A0 = Sortie avec resistance
const int ldrPin = A0;
int limitPhotoResistor = 600;

// IMU : D1(5) = SLC | D2(4) = SDA | 3V3 = VCC | GND = GND
const int MPU_ADDR = 0x68; // I2C address of the MPU-6050. If AD0 pin is set to HIGH, the I2C address will be 0x69.

// WIFI
// const char* ssid = "AP_Thomas";
// const char* password = "122345678";
const char* ssid = "Samsung guillaume";
const char* password = "vwic8832";

// MQTT
const String publishTopic = "STC/" + String(ESPSerialNumber);
const String subscribeTopic = publishTopic;
EspMQTTClient client(
  ssid,
  password,
  "51.75.121.250",  // MQTT Broker server ip
  "",   // Username Can be omitted if not needed
  "",   // Password Can be omitted if not needed
  ESPSerialNumber,      // Client name that uniquely identify your device
  8883
);

bool trashCanIsFull = false;
 
void setup() 
{
  Serial.begin(115200);
  Serial.println();
  Serial.println("====Projet DEV - Setup====");

  initSensor();

  initLed();

  connectWifi();

  subscribeTopics();

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

  digitalWrite(ledPin, HIGH);
  delay(2000);
  digitalWrite(ledPin, LOW);
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
  
  // Fait clignoter la led 3x
  for(int i =0; i < 3;i++){
    digitalWrite(ledPin, HIGH);
    delay(200);    
    digitalWrite(ledPin, LOW); 
    delay(200);    
  }

}

bool subscribeTopics(){

  if(client.isConnected()){

    client.subscribe(subscribeTopic, [] (const String &payload)  {
      Serial.print("Message received with topic '" + subscribeTopic + "' : " );
      Serial.println(payload);

      int index1 = payload.indexOf(':');
      int index2 = payload.indexOf(',');
      String value = payload.substring(index1 +1, index2);
      value.replace(" ", "");

      if(value == "0"){
        trashCanIsFull = false;
      }else if(value == "1"){
        trashCanIsFull = true;
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
  ledPin = 5;

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
  ledPin = 5;

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
  ledPin = 2;

  Wire.begin();
  Wire.beginTransmission(MPU_ADDR); // Begins a transmission to the I2C slave (GY-521 board)
  Wire.write(0x6B); // PWR_MGMT_1 register
  Wire.write(0); // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
}

bool checkInclination(){
  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x3B); // starting with register 0x3B (ACCEL_XOUT_H) [MPU-6000 and MPU-6050 Register Map and Descriptions Revision 4.2, p.40]
  Wire.endTransmission(false); // the parameter indicates that the Arduino will send a restart. As a result, the connection is kept active.
  Wire.requestFrom(MPU_ADDR, 2*2, true); // request a total of 7*2=14 registers
  
  // "Wire.read()<<8 | Wire.read();" means two registers are read and stored in the same variable
  int16_t accelerometer_x = Wire.read()<<8 | Wire.read(); // reading registers: 0x3B (ACCEL_XOUT_H) and 0x3C (ACCEL_XOUT_L)
  int16_t accelerometer_y = Wire.read()<<8 | Wire.read(); // reading registers: 0x3D (ACCEL_YOUT_H) and 0x3E (ACCEL_YOUT_L)  

  return accelerometer_x < 1000 && accelerometer_y < -8000;
}

bool checkFlat(){
  Wire.beginTransmission(MPU_ADDR);
  Wire.write(0x3B); // starting with register 0x3B (ACCEL_XOUT_H) [MPU-6000 and MPU-6050 Register Map and Descriptions Revision 4.2, p.40]
  Wire.endTransmission(false); // the parameter indicates that the Arduino will send a restart. As a result, the connection is kept active.
  Wire.requestFrom(MPU_ADDR, 2*2, true); // request a total of 7*2=14 registers
  
  // "Wire.read()<<8 | Wire.read();" means two registers are read and stored in the same variable
  int16_t accelerometer_x = Wire.read()<<8 | Wire.read(); // reading registers: 0x3B (ACCEL_XOUT_H) and 0x3C (ACCEL_XOUT_L)
  int16_t accelerometer_y = Wire.read()<<8 | Wire.read(); // reading registers: 0x3D (ACCEL_YOUT_H) and 0x3E (ACCEL_YOUT_L)  

  return accelerometer_x < 4000 && accelerometer_x > -4000 && accelerometer_y < 4000 && accelerometer_y > -4000;
}

void checkIMU(){
  
  if(checkInclination())
  {
    Serial.println("Inclinée");

    while(!checkFlat())    
    {
      delay(500);  
    }
    
    Serial.println("A plat");  

    trashCanIsFull = !trashCanIsFull;
    publishMeasure(); 
    Serial.print("La poubelle est : ");
    Serial.println(trashCanIsFull);
  }
}

// ===== MQTT =====

bool publishMeasure() {
  if(client.isConnected()){
    String strState = trashCanIsFull ? "1" : "0";    
    client.publish(publishTopic, "{\"value\":" + strState + ",\"sn\":\"" + String(ESPSerialNumber) + "\"}", true);

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
