using UnityEngine;
using ArduinoBluetoothAPI;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using UnityEngine.SceneManagement;

public class TestBluetooth : MonoBehaviour
{
    public static TestBluetooth BTHInstance;
    [SerializeField] private Transform RHController;
    private static Transform rightHandController;

    private static readonly float dmax = 0.05f;
    private const float coeffReduc = 0.1f;

    public bool simpleMode;

    private void Awake()
    {
        if (BTHInstance && BTHInstance != this)
            Destroy(gameObject);
        BTHInstance = this;

        BluetoothHandler.Init();
        rightHandController = RHController;
    }

    private void FixedUpdate()
    {
        var objects = rightHandController.GetComponent<XRDirectInteractor>().interactablesSelected;
        
        if (objects.Count > 0)
        {
            Rigidbody rgbd = objects[0].transform.gameObject.GetComponent<Rigidbody>();
            decimal mass = (decimal)rgbd.mass;
        
            if (simpleMode)
            {
                BluetoothHandler.SendData(((float)mass) / 2.0f, ((float)mass) / 2.0f);

                print((float)mass / 2.0f);

                return;
            }

            Vector3 pos = rgbd.worldCenterOfMass - rightHandController.position;
        
            float d = coeffReduc * Vector2.Distance(
                new Vector2(rgbd.worldCenterOfMass.x, rgbd.worldCenterOfMass.z),
                new Vector2(rightHandController.position.x, rightHandController.position.z)
            );

            float tw1 = Mathf.Rad2Deg * Mathf.Asin(pos.x / d), tw2 = Mathf.Rad2Deg * Mathf.Acos(pos.z / d);
            float thetaWorld = Mathf.Atan2(pos.x / d, pos.z / d) * Mathf.Rad2Deg;
            float PW = rightHandController.parent.parent.rotation.eulerAngles.y;
            float CP = rightHandController.localEulerAngles.y;

            float alpha = thetaWorld - (CP+PW);
            alpha = alpha - 360 * (int)(alpha / 360);
            if (alpha < 0)
                alpha += 360;

            float x = Mathf.Cos(Mathf.Deg2Rad * alpha) * d;
            float m1, m2;

            float ratio = x / dmax * 0.5f + 0.5f;

                
            m1 = (float)mass * ratio;
            m2 = (float)mass * (1-ratio);

        
            print($"m1: {m1}, m2: {m2}, x: {x}, alpha: {alpha}");

            BluetoothHandler.SendData(m1, m2);
        }
        else
        {
            BluetoothHandler.SendData(0, 0);
        }

    }
}
