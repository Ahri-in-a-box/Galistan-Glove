//end switch
#define SW0 32
#define SW1 33

#define PUL0 27  //Pulse pin
#define DIR0 26  //Direction pin
#define ENA0 25  //enable pin

#define PUL1 21  //Pulse pin
#define DIR1 19  //Direction pin
#define ENA1 18  //enable pin

//number of steps to make 1/200 of a turn
#define MicroStep 32

#include <AccelStepper.h>

//Definition of our two stepper motors
AccelStepper M0 = AccelStepper(AccelStepper::DRIVER, PUL0, DIR0, -1, -1, true);
AccelStepper M1 = AccelStepper(AccelStepper::DRIVER, PUL1, DIR1, -1, -1, true);

bool WeightChanged = true;

//How much ml we inject per step of a motor
#define MLPERSTEP 0.005454545454545f

void OnRX() {
  ushort m0, m1, a;
  float l0, l1;

  //only read data if available is at least one packet long
  while (Serial.available() >= 7) {

    //only read data if packet start with 0x42 and Ox69
    if (Serial.read() != 0x42)
      continue;

    if (Serial.read() != 0x69)
      continue;

    //read 2 bytes fields (unsigned short) in the packet
    m0 = Serial.read() | Serial.read() << 8;
    m1 = Serial.read() | Serial.read() << 8;

    //read empty byte at the end
    Serial.read();

    //convert grams sent by unity to steps
    l0 = m0 / MLPERSTEP;
    l1 = m1 / MLPERSTEP;

    //scale data since water is not heavy enough to have heavy objects
    l0 *= 1.6f;
    l1 *= 1.6f;

    //prevent the motors from pushing beyond the end
    if (l0 > 11000)
      l0 = 11000;

    if (l1 > 11000)
      l1 = 11000;

    //Multiply the number of step by the microstepping factor and set it as objective
    M0.moveTo((uint32_t)l0 * MicroStep);
    M1.moveTo((uint32_t)l1 * MicroStep);
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
  M0.setAcceleration(1000 * MicroStep);
  M1.setAcceleration(1000 * MicroStep);

  //Use Serial port connected to the other card to get the data
  Serial.begin(115200);

  //Perform one home
  HomeActuators();

  //Set speed of motors
  M0.setMaxSpeed(3000 * MicroStep);
  M1.setMaxSpeed(3000 * MicroStep);

  //Set target position to 0;
  M0.moveTo(0);
  M1.moveTo(0);
}

// Used to home the two linear actuators
void HomeActuators() {
  //Set low speed to home with more precision
  M0.setMaxSpeed(500 * MicroStep);
  M1.setMaxSpeed(500 * MicroStep);

  //Move a lot to be sure to reach end of the actuator
  M0.moveTo(20000 * MicroStep);
  M1.moveTo(20000 * MicroStep);

  //make motor run while the switch are not pressed
  while (!digitalRead(SW0) || !digitalRead(SW1)) {
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

  //Check for serial data
  OnRX();

  //turn led on or off according to switch state (used to check that the switches work)
  digitalWrite(LED_BUILTIN, digitalRead(SW0) ^ digitalRead(SW1));

  //make motors perform a step if necessary
  M0.run();
  M1.run();
}