using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using UnityEngine.XR.Interaction.Toolkit;

public class TestBluetooth : MonoBehaviour
{
    public static TestBluetooth BTHInstance;

    private static BluetoothHelper BTHelper;
    private static byte[] data = new byte[512];
    private int lastDataSent = 0;

    //[SerializeField, Range(0, 3000)] private int dataToSend = 500;
    public static int dataToSend = 1;

    private void Awake()
    {
        if (BTHInstance != null)
            Destroy(gameObject);
        else
            BTHInstance = this;

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
        /*
        if(BTHelper.isConnected() && lastDataSent != dataToSend)
        {
            SendWeight(dataToSend);
            lastDataSent = dataToSend;
        }
        while (BTHelper.Available)
        {
            print(BTHelper.ReadBytes().Length);
        }*/
    }


    public static void SendContainerWeight(GameObject obj)
    {
        Rigidbody rgbd = obj.GetComponent<Rigidbody>();
        decimal mass = (decimal)rgbd.mass;
        
        if (rgbd.tag == "Container")
            mass += rgbd.gameObject.GetComponentInChildren<CollectorBucketBehavior>().GetMass();
        
        SendWeight((int)(mass * 1000));
    }

    public static void SendObjectWeight(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.tag != "Ground")
        {
            SendContainerWeight(args.interactableObject.transform.gameObject);
        }
    }

    public static void ResetObjectWeight(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.tag != "Ground")
            SendWeight(1);
    }


    private static void SendWeight(int mass)
    {
        ushort d = (ushort)mass;
        data[2] = (byte)(d & 0xff);
        data[3] = (byte)(d >> 8);

        BTHelper.SendData(data[0..4]);
        print("Sent" + mass);
    }


}
