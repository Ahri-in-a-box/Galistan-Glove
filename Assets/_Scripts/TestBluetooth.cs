using UnityEngine;
using ArduinoBluetoothAPI;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class TestBluetooth : MonoBehaviour
{
    public static TestBluetooth BTHInstance = new();
    [SerializeField] private Transform RHController;
    private static Transform rightHandController;

    private static readonly float dmax = 0.05f;
    private const float coeffReduc = 0.1f;

    private GameObject props;

    private void Awake()
    {
        BluetoothHandler.Init();
        rightHandController = RHController;
    }

    private void FixedUpdate()
    {
        if (props)
        {
            Rigidbody rgbd = props.GetComponent<Rigidbody>();
            decimal mass = (decimal)rgbd.mass;
        
            if (rgbd.tag == "Container")
                mass += rgbd.gameObject.GetComponentInChildren<CollectorBucketBehavior>()?.GetMass() ?? 0;
        
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

    }

    public void SendContainerWeight(GameObject obj)
    {
        props = obj;
    }

    public void SendObjectWeight(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.tag != "Ground")
            SendContainerWeight(args.interactableObject.transform.gameObject);
    }

    public void ResetObjectWeight(SelectExitEventArgs args)
    {
        props = null;
        if (args.interactableObject.transform.tag != "Ground")
            BluetoothHandler.SendData(0, 0);
    }


}
