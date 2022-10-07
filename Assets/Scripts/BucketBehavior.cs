using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BucketBehavior : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private float mass = 0.0f;
    
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment")
            mass += other.GetComponent<Rigidbody>().mass;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment")
            mass -= other.GetComponent<Rigidbody>().mass;
    }

    public float GetMass()
    {
        return mass;
    }
}
