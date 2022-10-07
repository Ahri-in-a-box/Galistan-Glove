using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoCommunicationManager : MonoBehaviour
{
    private static ArduinoCommunicationManager instance;
    private static readonly SerialPort arduino = new("COM3", 115200);
    private static byte[] data = new byte[512];

    [SerializeField, Range(0, 3000)]
    private float m_DataToSend = 0x0123;
    private float lastSend = -1;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        arduino.Open();
        data[0] = 0x42;
        data[1] = 0x69;
    }

    private void FixedUpdate()
    {
        SendWeight(m_DataToSend / 1000);
    }

    public static void SendWeight(float weight)
    {
        ushort d = (ushort)(weight * 1000);
        data[2] = (byte)(d & 0xFF);
        data[3] = (byte)(d >> 8);
        arduino.Write(data, 0, 4);
    }
}