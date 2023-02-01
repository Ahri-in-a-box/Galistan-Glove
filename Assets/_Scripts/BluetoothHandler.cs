using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;

public static class BluetoothHandler
{
    private static BluetoothHelper BTHelper;
    private static readonly byte[] data = new byte[7];

    private static bool init = false;

    public enum BLState
    {
        Disconnected,
        Connecting,
        Connected
    }

    public static BLState state = BLState.Disconnected;
    public delegate void StateChange();
    public static event StateChange OnStateChange = null;

    public delegate void DataReceived(byte[] data);
    public static event DataReceived OnDataReceived = null;


    public static void Init()
    {
        if (init)
            return;

        BTHelper = BluetoothHelper.GetInstance("Galistan-Glove");

        BTHelper.OnConnectionFailed += BTHelper_OnConnectionFailed;
        BTHelper.OnConnected += BTHelper_OnConnected;
        BTHelper.OnDataReceived += BTHelper_OnDataReceived;

        ActivateWeight();

        data[0] = 0x42;
        data[1] = 0x69;

        init = true;
    }

    private static void BTHelper_OnDataReceived()
    {
        OnDataReceived?.Invoke(BTHelper.ReadBytes());
    }

    private static void BTHelper_OnConnected()
    {
        Debug.Log("On connected");
        BTHelper.StartListening();
        state = BLState.Connected;
        OnStateChange?.Invoke();
    }

    private static void BTHelper_OnConnectionFailed()
    {
        Debug.Log("On connection Failed");
        state = BLState.Disconnected;
        OnStateChange?.Invoke();
    }

    public static void DeactivateWeight()
    {
        SendData(0, 0);
        BTHelper.Disconnect();
        state = BLState.Disconnected;
        OnStateChange?.Invoke();
    }

    public static void ActivateWeight()
    {
        BTHelper.Connect();
        BTHelper.setFixedLengthBasedStream(7);
        state = BLState.Connecting;
        OnStateChange?.Invoke();
    }

    public static void SendData(float m1, float m2)
    {
        ushort d1 = (ushort)(m1 * 1000);
        ushort d2 = (ushort)(m2 * 1000);

        data[1] = 0x69;
        data[2] = (byte)(d1 & 0xff);
        data[3] = (byte)(d1 >> 8);
        data[4] = (byte)(d2 & 0xff);
        data[5] = (byte)(d2 >> 8);

        if (BTHelper.isConnected())
        {
            Debug.Log($"Sent: ({m1}, {m2})");
            BTHelper.SendData(data);
        }
        else
            Debug.Log("Arduino not available");
    }

    public static void SendCalibrate()
    {
        data[1] = 0x70;
        data[2] = 0xff;
        data[3] = 0xff;
        data[4] = 0xff;
        data[5] = 0xff;

        if (BTHelper.isConnected())
        {
            Debug.Log($"Sent Calibration");
            BTHelper.SendData(data);
        }
        else
            Debug.Log("Arduino not available");
    }

}
