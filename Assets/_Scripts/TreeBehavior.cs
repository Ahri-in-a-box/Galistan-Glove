using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    private int count = 0;
    private bool isFallen = false;

    public delegate void OnFall();
    public static event OnFall OnFallEvent;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "axe")
        {
            //print("arbre touché");
            count++;
        }
        if(count == 3)
        {
            //print("arbre coupé");
            isFallen = true;
            
            OnFallEvent?.Invoke();

            gameObject.GetComponent<Rigidbody>().isKinematic = false ;            
        }
        if(isFallen == true)
        {
            count = -1;
        }
    }

}
