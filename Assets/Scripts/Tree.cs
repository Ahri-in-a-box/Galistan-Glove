using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int count = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "axe")
        {
            print("arbre touché");
            count++;
        }
        if(count == 3)
        {
            print("arbre coupé");
            count = 0;
            gameObject.GetComponent<Rigidbody>().isKinematic = false ;
        }
    }
}
