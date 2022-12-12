using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControl_ : MonoBehaviour
{
    public Hand_ hand;
    public Hand_ handPhysics;

    public float curl0;
    public float curl1;
    public float curl2;
    public float curl3;
    public float curl4;
    // Start is called before the first frame update
    void Start()
    {
        BluetoothHandler.OnDataReceived += ReadCurls;
    }

    private void ReadCurls(byte[] data)
    {
        if (data.Length != 8)
        {
            print($"curl's length : {data.Length}");
            return;
        }

        if (data[0] != 0x42 || data[1] != 0x69)
        {
            print($"data : {data[0]} data[1] : {data[1]}");
            return;
        }

        curl0 = data[2] / 255.0f;
        curl1 = data[3] / 255.0f;
        curl2 = data[4] / 255.0f;
        curl3 = data[5] / 255.0f;
        curl4 = data[6] / 255.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //print(curl0 + " " + curl1 + " " + curl2 + " " + curl3 + " " + curl4);

        hand.SetThumb(curl0);
        handPhysics.SetThumb(curl0);
        hand.SetIndex(curl1);
        handPhysics.SetIndex(curl1);
        hand.SetMiddle(curl2);
        handPhysics.SetMiddle(curl2);
        hand.SetRing(curl3);
        handPhysics.SetRing(curl3);
        hand.SetPinky(curl4);
        handPhysics.SetPinky(curl4);

    }


    private void OnTriggerStay(Collider other)
    {
        XRGrabInteractable var = other.gameObject.GetComponent<XRGrabInteractable>();
        if (var is null)
            return;
        if(curl1 > 0.4f && var.isActiveAndEnabled)
        {
            GetComponent<XRDirectInteractor>().StartManualInteraction(other.GetComponent<IXRSelectInteractable>());
        }
        else
        { 
            GetComponent<XRDirectInteractor>().EndManualInteraction();            
        }
    }

    private void OnDestroy()
    {
        BluetoothHandler.OnDataReceived -= ReadCurls;
    }
}
