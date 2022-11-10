//number of steps that the stepper motor need to reach
uint32_t Po1, Po2;

//Driver (pin a  changer)
#define PUL1 27  //Pulse pin
#define DIR1 26  //Direction pin (LOW = forward, HIGH = backward)
#define ENA1 25  //enable pin

#define PUL2 32  //Pulse pin
#define DIR2 12  //Direction pin (LOW = forward, HIGH = backward)
#define ENA2 34  //enable pin

//number of steps to make 1/200 of a turn
#define MicroStep 32

#include "BluetoothSerial.h"
#include <AccelStepper.h>

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

BluetoothSerial SerialBT = BluetoothSerial();

//Definition of our two stepper motors
AccelStepper M1 = AccelStepper(AccelStepper::DRIVER, PUL1, DIR1, -1, -1, true);
AccelStepper M2 = AccelStepper(AccelStepper::DRIVER, PUL2, DIR2, -1, -1, true);

//How much ml we inject per step of a motor
#define MLPERSTEP 0.007038;

void OnRX() {
  ushort m1, m2, a;
  float l1, l2;

  while (SerialBT.available() >= 8) {
    if (SerialBT.read() != 0x42)
      continue;

    if (SerialBT.read() != 0x69)
      continue;

    m1 = SerialBT.read() | SerialBT.read() << 8;
    m2 = SerialBT.read() | SerialBT.read() << 8;

    //not Used
    a = SerialBT.read() | SerialBT.read() << 8;

    m1 /=3;
    m2 /=3;

    //convert grams sent by unity to steps
    l1 = m1 / 6.44f;
    l1 /= MLPERSTEP;

    l2 = m2 / 6.44f;
    l2 /= MLPERSTEP;

    //prevent the motors from pushing beyond the end
    if (l1 > 25000)
      l1 = 25000;
    
    if (l2 > 25000)
      l2 = 25000;

    //Multiply the number of step by the microstepping factor and set it as objective
    Po1 = (uint32_t)l1 * MicroStep;
    Po2 = (uint32_t)l2 * MicroStep;
  }
}

void setup() {
  // put your setup code here, to run once:

  //enable drivers
  pinMode(ENA1, OUTPUT);
  pinMode(ENA2, OUTPUT);
  digitalWrite(ENA1, LOW);
  digitalWrite(ENA2, LOW);

  //Set speed of motors and acceleration
  M1.setMaxSpeed(2800 * MicroStep);
  M2.setMaxSpeed(2800 * MicroStep);
  M1.setAcceleration(250 * MicroStep);
  M2.setAcceleration(250 * MicroStep);

  //Enable bluetooth
  SerialBT.begin("Galistan-Glove");  //Bluetooth device name
  SerialBT.enableSSP();

  //Set target position to 0;
  Po1 = 0;
  Po2 = 0;
}

void loop() {

  //Check for bluetooth data
  OnRX();

  //update motors target position
  M1.moveTo(Po1);
  M2.moveTo(Po2);

  //make motors perform a step if necessary
  M1.run();
  M2.run();
}