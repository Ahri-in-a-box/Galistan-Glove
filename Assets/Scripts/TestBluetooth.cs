using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;

public class TestBluetooth : MonoBehaviour
{
    private static BluetoothHelper BTHelper;
    private static byte[] data = new byte[512];
    private int lastDataSent = 0;

    //[SerializeField, Range(0, 3000)] private int dataToSend = 500;
    public static int dataToSend = 1;

    private void Awake()
    {
        BTHelper = BluetoothHelper.GetInstance("Galistan-Glove");
        BTHelper.OnConnectionFailed += BTHelper_OnConnectionFailed;
        BTHelper.OnConnected += BTHelper_OnConnected;
        BTHelper.OnDataReceived += BTHelper_OnDataReceived;

        BTHelper.Connect();
        
        BTHelper.setFixedLengthBasedStream(4);

        data[0] = 0x42;
        data[1] = 0x69;
    }

    private void BTHelper_OnDataReceived()
    {
        
    }

    private void BTHelper_OnConnected()
    {
        print("On connected");
        BTHelper.StartListening();
    }

    private void BTHelper_OnConnectionFailed()
    {
        print("On connection Failed");
    }

    private void FixedUpdate()
    {
        if(BTHelper.isConnected() && lastDataSent != dataToSend)
        {
            SendWeight(dataToSend);
            lastDataSent = dataToSend;
        }
        while (BTHelper.Available)
        {
            print(BTHelper.ReadBytes().Length);
        }
    }

    public static void SendWeight(int mass)
    {
        ushort d = (ushort)mass;
        data[2] = (byte)(d & 0xff);
        data[3] = (byte)(d >> 8);

        BTHelper.SendData(data[0..4]);
        print("Sent");
    }


}
