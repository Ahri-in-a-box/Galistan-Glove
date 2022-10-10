using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class BucketBehavior : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    List<GameObject> objects = new List<GameObject>();
    private decimal mass = 0.0M;
    public bool isChanged = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "0.0 Kg";
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(GetMass().ToString() + " Kg");
        if(isChanged)
        {
            isChanged = false;
            Invoke("OnSelectExited", 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment")
        {
            mass += (decimal)other.GetComponent<Rigidbody>().mass;
            other.transform.SetParent(transform);
            objects.Add(other.gameObject);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment")
        {
            mass -= (decimal)other.GetComponent<Rigidbody>().mass;
            other.transform.SetParent(null);
            objects.Remove(other.gameObject);
        }
    }

    public void OnSelectEntered()
    {
        foreach(GameObject obj in objects)
        {
            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.GetComponent<Rigidbody>().useGravity = false;
        }
 
        isChanged = true;
        print("Activé");
    }

    public void OnSelectExited()
    {
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().useGravity = true;
        }
        print("Désactivé");
    }

    public decimal GetMass()
    {
        return mass;
    }
}
