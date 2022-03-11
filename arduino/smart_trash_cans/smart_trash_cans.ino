
const int TRASHCANS1_LED = 1;
const int TRASHCANS1_SENSOR = 11;

const int TRASHCANS2_LED = 2;
const int TRASHCANS2_SENSOR = 12;

const int TRASHCANS3_LED = 3;
const int TRASHCANS3_SENSOR = 13;


void setup() {
  Serial.begin(115200);

  //TODO : pin mode all pins
  
}

void loop() {

  sendMeasures();

  delay(2000);
}

void sendMeasures(){
  int valueSensorTrashCans1 = digitalRead(TRASHCANS1_SENSOR);
  int valueSensorTrashCans2 = digitalRead(TRASHCANS2_SENSOR);
  int valueSensorTrashCans3 = digitalRead(TRASHCANS3_SENSOR);
  
  checkTrashCansIsFull();

  sendMQTT("topic", "body");
}

void sendMQTT(String topic, String body){
  //todo
}

void checkTrashCansIsFull(){
  //todo check sensors values + enabled led if is full
}