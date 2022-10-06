using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System.Threading;
using System.IO.Ports;

public class ArduinoCommunicationManager : MonoBehaviour
{
    private static ArduinoCommunicationManager instance;
    private static SerialPort arduino = new SerialPort("COMX", 9600);

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        arduino.Open();
    }

    private static string SerializeData(object gravityCenter = null, object weight = null)
    {
        string res = "{";
        if (weight is not null)
            res += "weight:" + ((float)weight).ToString() + ";";
        if (gravityCenter is not null)
        {
            Vector3 gc = (Vector3)gravityCenter;
            res += "gravityCenter:" + gc.x + "," + gc.y + "," + gc.z + ";";
        }
        res += "}";
        return res;
    }

    public static void SendWeight(float weight)
    {
        arduino.Write(SerializeData(weight: weight));
    }

    public static void SendGravityCenter(Vector3 gravityCenter)
    {
        arduino.Write(SerializeData(gravityCenter));
    }

    public static void SendGravityAndWeight(Vector3 gravityCenter, float weight)
    {
        arduino.Write(SerializeData(gravityCenter, weight));
    }
}