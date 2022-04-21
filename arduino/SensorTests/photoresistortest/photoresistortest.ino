const int ldrPin = A0;

bool trashCanIsFull = false;
int limitPhotoResistor = 600;

void setup(){
  Serial.begin(115200);
  pinMode(ldrPin, INPUT);
}

void loop(){
  
  checkTrashCan2();

  delay(1000);
}

void checkTrashCan2(){

  if(trashCan2IsOpen()){
    Serial.println("La poubelle est ouverte");

    while(trashCan2IsOpen()){
      delay(500);
    }

    Serial.println("La poubelle est refermée");
    trashCanIsFull = !trashCanIsFull;
  }
  
  Serial.print("Etat de la poubelle : ");
  Serial.println(trashCanIsFull);
}

bool trashCan2IsOpen(){

  Serial.println(analogRead(ldrPin));
  
  return analogRead(ldrPin) >= limitPhotoResistor ? true : false; 
}