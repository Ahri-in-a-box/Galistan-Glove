#include "BluetoothSerial.h"
#include <AccelStepper.h>

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

BluetoothSerial Bluetooth = BluetoothSerial();

//list of pins used by the potentiometers
int pins[] = { 27, 26, 25, 33, 32 };

//list of max value seen by each potentiometers
int maxs[] = { 0, 0, 0, 0, 0 };

//listo of avg values at the start of the calibration for each potentiometers
float avg[] = { 0, 0, 0, 0, 0 };

//list of min value seen by each potentiometers
int mins[] = { 4095, 4095, 4095, 4095, 4095 };

//buffer used to send the data to unity over bluetooth
uint8_t data[7] = { 0x42, 0x69, 0, 0, 0, 0, 0 };

//every 100 iteration we update the calibration
int idx = 0;

void setup() {

  //Enable bluetooth
  Bluetooth.begin("Galistan-Glove");
  Bluetooth.enableSSP();

  //Enable serial with the second esp32
  Serial.begin(115200);

  for (int i = 0; i < sizeof(pins) / sizeof(int); i++) {
    pinMode(pins[i], INPUT);
  }

  CalibrationStart();
}

void CalibrationStart() {

  //set avg values to 0 for each pins
  for (int i = 0; i < sizeof(pins) / sizeof(int); i++)
    avg[i] = 0;

  //calculate avg values for current finger position
  for (int s = 0; s < 100; s++)
    for (int i = 0; i < sizeof(pins) / sizeof(int); i++)
      avg[i] += (4095 - analogRead(pins[i])) / 100.0f;

  //set the min and the max to the current position plus or minus one, this allow the dynamic calibration to then reduce min to the correct value and increase max to the correct value
  for (int i = 0; i < sizeof(pins) / sizeof(int); i++) {
    mins[i] = avg[i] - 1;
    maxs[i] = avg[i] + 1;
    avg[i] = 0;
  }

  idx = 0;
}

void OnRX() {

  //just transfert data from bluetooth to the second esp32
  while (Bluetooth.available() >= 7) {

    //only read data if packet start with 0x42 and Ox69
    if (Bluetooth.read() != 0x42)
      continue;

    static uint8_t ID;

    //this value is either 0x69 or 0x70, else the data is invalid
    ID = Bluetooth.read();

    switch (ID) {
      //weight that we send on the serial port to the second esp32
      case 0x69:
        Serial.write(0x42);
        Serial.write(0x69);

        //send remaining data
        for (uint8_t i = 0; i < 5; i++)
          Serial.write(Bluetooth.read());

        Serial.flush();
        break;
      //Indicate that we should redo the calibration
      case 0x70:

        // the rest of the data should be 0xff
        for (uint8_t i = 0; i < 5; i++)
          if (Bluetooth.read() != 0xff)
            break;

        CalibrationStart();
        break;
    }
  }
}

//Last time that we sent potentiometers values to unity
unsigned long lastUpdate = 0;

//add a dead zone to the value
int AddDeadZones(int value, int size, int i) {
  value = map(value, mins[i], maxs[i], 0, 255 + size * 2);

  //new value is between 0 and 255 + size * 2, we now want to make size first values equal to 0 and size last values equal to 255
  if (value > 255 + size)
    value = 255 + size;
  if (value < size)
    value = size;

  return value - size;
}

void loop() {

  OnRX();

  //10 times per seconds we send the position of each fingers
  if (millis() - lastUpdate >= 10) {
    lastUpdate = millis();

    if (++idx > 100)
      idx -= 100;

    static int tmp;

    //foreach pins we update the average and if we are at the 100's value we update calibration
    for (int i = 0; i < sizeof(pins) / sizeof(int); i++) {
      tmp = 4095 - analogRead(pins[i]);

      avg[i] += tmp / 100.0f;

      if (idx == 100) {
        mins[i] = min((int)avg[i], mins[i]);
        maxs[i] = max((int)avg[i], maxs[i]);
        avg[i] = 0;
      }

      //Prevent the current value from being outside of the current calibration
      tmp = max(min(tmp, maxs[i]), mins[i]);

      //remap the value to add dead zone allowing a fully close / open finger even if we are not at the max or min value
      data[i + 2] = AddDeadZones(tmp, 16, i);
    }

    if (Bluetooth.hasClient()) {
      Bluetooth.write(data, 7);
      Bluetooth.flush();
    }
  }
}