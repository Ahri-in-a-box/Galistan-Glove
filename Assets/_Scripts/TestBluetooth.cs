using UnityEngine;
using ArduinoBluetoothAPI;
using UnityEngine.XR.Interaction.Toolkit;

public class TestBluetooth : MonoBehaviour
{
    public static TestBluetooth BTHInstance;

    [SerializeField] private Transform RHController;
    private static Transform rightHandController;

    private static BluetoothHelper BTHelper;
    private static readonly byte[] data = new byte[512];
    private static readonly float dmax = 0.05f;
    private const float coeffReduc = 0.1f;

    private GameObject props;
    private int timeCount = 0;

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
        
        BTHelper.setFixedLengthBasedStream(8);

        data[0] = 0x42;
        data[1] = 0x69;

        rightHandController = RHController;
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
        
            SendData(m1, m2);
        
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
            SendData(0, 0);
    }

    private static void SendData(float m1, float m2)
    {
        ushort d1 = (ushort)(m1 * 1000);
        ushort d2 = (ushort)(m2 * 1000);

        data[2] = (byte)(d1 & 0xff);
        data[3] = (byte)(d1 >> 8);
        data[4] = (byte)(d2 & 0xff);
        data[5] = (byte)(d2 >> 8);

        if (BTHelper.isConnected())
            BTHelper.SendData(data[0..8]);
        else
            print("Arduino not available");
    }


}
