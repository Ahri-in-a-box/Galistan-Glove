volatile ushort del = 65535;

void OnRX()
{
  while (Serial.available() >= 4)
  {
    if (Serial.read() != 0x42)
      continue;

    if (Serial.read() != 0x69)
      continue;

    del = Serial.read() | Serial.read() << 8;
  }
}

void setup() {
  // put your setup code here, to run once:
  pinMode(LED_BUILTIN, OUTPUT);

  Serial.begin(115200);
  Serial.onReceive(OnRX);

  while (del == 65535);
}

void loop() {
  // put your main code here, to run repeatedly:
  digitalWrite(LED_BUILTIN, 0);
  delay(del);
  digitalWrite(LED_BUILTIN, 1);
  delay(del);
}
