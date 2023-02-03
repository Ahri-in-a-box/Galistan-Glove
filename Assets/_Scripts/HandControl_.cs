using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControl_ : MonoBehaviour
{
    public Hand_ hand;

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
        if (data.Length != 7)
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
        hand.SetIndex(curl1);
        hand.SetMiddle(curl2);
        hand.SetRing(curl3);
        hand.SetPinky(curl4);

        

        if ((GetComponent<XRDirectInteractor>().isPerformingManualInteraction) && (curl0 <= 0.4f || (curl1 <= 0.4f && curl2 <= 0.4f && curl3 <= 0.4f && curl4 <= 0.4f)))
        {
            GetComponent<XRDirectInteractor>().EndManualInteraction();
        }

    }


    private void OnTriggerStay(Collider other)
    {
        XRGrabInteractable var = other.gameObject.GetComponent<XRGrabInteractable>();
        
        if (var is null || !var.isActiveAndEnabled || other.gameObject.tag == "Teleporter")
            return;
        if((curl0 > 0.4f && !GetComponent<XRDirectInteractor>().isPerformingManualInteraction) && (curl1 > 0.4f || curl2 > 0.4f || curl3 > 0.4f || curl4 > 0.4f))
        {
            GetComponent<XRDirectInteractor>().StartManualInteraction(other.GetComponent<IXRSelectInteractable>());
        }
    }

    private void OnDestroy()
    {
        BluetoothHandler.OnDataReceived -= ReadCurls;
    }

   
}
