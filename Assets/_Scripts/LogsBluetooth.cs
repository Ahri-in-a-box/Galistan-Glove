using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using TMPro;

public class LogsBluetooth : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textLogs;


    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string errorText, string stackTrace, LogType type)
    {
        if(type == LogType.Error)
        {
            textLogs.text += errorText + '\n';
        }
    }

}
