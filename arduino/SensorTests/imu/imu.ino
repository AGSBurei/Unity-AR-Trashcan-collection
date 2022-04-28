#include "Wire.h" // This library allows you to communicate with I2C devices.

// IMU
const int MPU_ADDR = 0x68; // I2C address of the MPU-6050. If AD0 pin is set to HIGH, the I2C address will be 0x69.

bool trashCanIsFull = false;

void setup(){
  Serial.begin(9600);
   
  initIMU();
}

void initIMU(){
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

void loop(){

  checkTrashCan3();

  delay(100);
}

// Fils de l'IMU sont coté inclinaison de la poubelle
void checkTrashCan3(){

  if(checkInclination())
  {
    Serial.println("Inclinée");

    while(!checkFlat())    
    {
      delay(500);  
    }
    
    Serial.println("A plat");  

    trashCanIsFull = !trashCanIsFull;
    Serial.print("La poubelle est : ");
    Serial.println(trashCanIsFull);
  }
  
}