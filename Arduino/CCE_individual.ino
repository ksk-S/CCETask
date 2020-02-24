const int motorPin1 = 3;
const int motorPin2 = 5;
char rc;

void setup() {
  pinMode(motorPin1, OUTPUT);
  pinMode(motorPin2, OUTPUT);
  Serial.begin(9600);
  Serial.println("<Arduino is ready>");
}

void loop() {
 recieveSerial();
}

void  recieveSerial() {

  while (Serial.available() > 0 ) {
    rc = Serial.read();

    if (rc == '1') {
      Vibrate1();
    }
    
    if (rc == '2') {
      Vibrate2();
    }    
  }
} 

void Vibrate1(){
  digitalWrite(motorPin1, HIGH);
  delay(50);
  digitalWrite(motorPin1, LOW);

}

void Vibrate2(){
  digitalWrite(motorPin2, HIGH);
  delay(50);
  digitalWrite(motorPin2, LOW);
}

