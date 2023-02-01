using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectorBucketBehavior : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private readonly List<GameObject> objects = new();
    private decimal mass = 0.0M;
    public bool isChanged = false;

    private bool isGrabbed = false;

    public delegate void OnRequiredApples();
    public static event OnRequiredApples OnRequiredApplesEvent;

    public delegate void OnNotEnoughApples();
    public static event OnNotEnoughApples OnNotEnoughApplesEvent;


    // Start is called before the first frame update
    void Start()
    {
        //text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if(text)
            text.text = "0.0 Kg";
    }

    // Update is called once per frame
    void Update()
    {
        if(text)
            text.SetText(GetMass().ToString() + " Kg");
        
        if(isChanged)
        {
            isChanged = false;
            Invoke("OnSelectExited", 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Apple")
        {
            mass += (decimal)other.GetComponent<Rigidbody>().mass;

            //update bucket's weight
            GetComponent<Rigidbody>().mass = (float)(mass + 0.2M);
            
            if (mass >= 1)
                OnRequiredApplesEvent?.Invoke();

            //if (isGrabbed && TestBluetooth.BTHInstance)
            //    TestBluetooth.BTHInstance.AddObjectWeight(transform.parent.gameObject);

            

            other.transform.SetParent(transform);
            objects.Add(other.gameObject);
        }    
    }

    private void OnTriggerExit(Collider other)
    {

        if(other.transform.parent == transform)
        {
            mass -= (decimal)other.GetComponent<Rigidbody>().mass;

            GetComponent<Rigidbody>().mass = (float)(mass + 0.2M);

            if (mass < 1)
                OnNotEnoughApplesEvent?.Invoke();

            //if (isGrabbed && TestBluetooth.BTHInstance)
            //    TestBluetooth.BTHInstance.AddObjectWeight(transform.parent.gameObject);
            
            other.transform.SetParent(null);
            objects.Remove(other.gameObject);
        }
    }

    public decimal GetMass()
    {
        return mass;
    }

    public void OnGrab()
    {
        isGrabbed = !isGrabbed;
    }

}
