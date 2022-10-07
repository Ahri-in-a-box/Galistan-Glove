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
        text.text = "0.0 g";
    }

    // Update is called once per frame
    void Update()
    {
        //text.SetText(CalculateWeight().ToString() + " g");
    }

  

    private float CalculateWeight()
    {
        return 0.0f;
    }
}
