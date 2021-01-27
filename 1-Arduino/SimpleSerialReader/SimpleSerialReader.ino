/*
  Software serial multple serial test

 Receives from the hardware serial, sends to software serial.
 Receives from software serial, sends to hardware serial.
 
 */
#include <SoftwareSerial.h>


SoftwareSerial mySerial(12, 11); // RX, TX

void setup()
{
  // Open serial communications and wait for port to open:
  Serial.begin(115200);
  //while (!Serial) {
  //  ; // wait for serial port to connect. Needed for Native USB only
  //}
  Serial.println("MCU Started!");

  // set the data rate for the SoftwareSerial port
  mySerial.begin(115200);
  mySerial.println("SoftwareSerial Initialised!");
}

void loop() // run over and over
{
 while (Serial.available() > 0)
   {
    char text = Serial.read();
    mySerial.print(text);
    
  }
}
