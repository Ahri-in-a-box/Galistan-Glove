//end switch
#define SW0 32
#define SW1 33

#define PUL0 27 //Pulse pin
#define DIR0 26 //Direction pin
#define ENA0 25 //enable pin

#define PUL1 21  //Pulse pin
#define DIR1 19  //Direction pin
#define ENA1 18  //enable pin

//number of steps to make 1/200 of a turn
#define MicroStep 32

#include "BluetoothSerial.h"
#include <AccelStepper.h>

#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif

BluetoothSerial SerialBT = BluetoothSerial();

//Definition of our two stepper motors
AccelStepper M0 = AccelStepper(AccelStepper::DRIVER, PUL0, DIR0, -1, -1, true);
AccelStepper M1 = AccelStepper(AccelStepper::DRIVER, PUL1, DIR1, -1, -1, true);

//How much ml we inject per step of a motor
#define MLPERSTEP 0.017595

//number of steps that the stepper motor need to reach
uint32_t Po0, Po1;

void OnRX() {
  ushort m0, m1, a;
  float l0, l1;

  //only read data if available is at least one packet long
  while (SerialBT.available() >= 8) {

    //only read data if packet start with 0x42 and Ox69
    if (SerialBT.read() != 0x42)
      continue;

    if (SerialBT.read() != 0x69)
      continue;

    //read 2 bytes fields (unsigned short) in the packet
    m0 = SerialBT.read() | SerialBT.read() << 8;
    m1 = SerialBT.read() | SerialBT.read() << 8;

    //not Used
    a = SerialBT.read() | SerialBT.read() << 8;

    m0 /=3;
    m1 /=3;

    //convert grams sent by unity to steps
    l0 = m0 / 6.44f;
    l0 /= MLPERSTEP;

    l1 = m1 / 6.44f;
    l1 /= MLPERSTEP;

    //prevent the motors from pushing beyond the end
    if (l0 > 11000)
      l0 = 11000;
    
    if (l1 > 11000)
      l1 = 11000;

    //Multiply the number of step by the microstepping factor and set it as objective
    Po0 = (uint32_t)l0 * MicroStep;
    Po1 = (uint32_t)l1 * MicroStep;
  }
}

void setup() {
  
  //configure switch as input pullup so that 0 = unpressed, 1 = pressed
  pinMode(SW0, INPUT_PULLUP);
  pinMode(SW1, INPUT_PULLUP);

  //blue led on the board
  pinMode(LED_BUILTIN, OUTPUT);

  //enable drivers
  pinMode(ENA0, OUTPUT);
  pinMode(ENA1, OUTPUT);
  digitalWrite(ENA0, HIGH);
  digitalWrite(ENA1, HIGH);

  //Set acceleration for smooth actuation
  M0.setAcceleration(300 * MicroStep);
  M1.setAcceleration(300 * MicroStep);

  //Enable bluetooth
  SerialBT.begin("Galistan-Glove");  //Bluetooth device name
  SerialBT.enableSSP();

  //Perform one home
  HomeActuators();

  //Set speed of motors and acceleration
  M0.setMaxSpeed(2800 * MicroStep);
  M1.setMaxSpeed(2800 * MicroStep);
  //Set target position to 0;
  Po0 = 0;
  Po1 = 0;
}

// Used to home the two linear actuators
void HomeActuators()
{
  //Set low speed to home with more precision
  M0.setMaxSpeed(500 * MicroStep);
  M1.setMaxSpeed(500 * MicroStep);

  //Move a lot to be sure to reach end of the actuator
  M0.moveTo(20000 * MicroStep);
  M1.moveTo(20000 * MicroStep);

  //make motor run while the switch are not pressed
  while (!digitalRead(SW0) || !digitalRead(SW1))
  {
    //turn led on or off according to switch state (used to check that the switches work)
    digitalWrite(LED_BUILTIN, digitalRead(SW0) ^ digitalRead(SW1));

    if (!digitalRead(SW0))
      M0.run();

    if (!digitalRead(SW1))
      M1.run();
  }

  //Set the position so that 0 = 60ml of liquid in the seringe
  M0.setCurrentPosition(11000 * MicroStep);
  M1.setCurrentPosition(11000 * MicroStep);
}

void loop() {

  //Check for bluetooth data
  OnRX();

  //turn led on or off according to switch state (used to check that the switches work)
  digitalWrite(LED_BUILTIN, digitalRead(SW0) ^ digitalRead(SW1));

  //update motors target position
  M0.moveTo(Po0);
  M1.moveTo(Po1);

  //make motors perform a step if necessary
  M0.run();
  M1.run();
}