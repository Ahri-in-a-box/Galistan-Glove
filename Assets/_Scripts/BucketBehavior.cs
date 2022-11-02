using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketBehavior : MonoBehaviour
{
    List<GameObject> objects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment" && other.gameObject.tag != "Tool")
        //if (other.gameObject.tag == "Apple")
        {
            other.transform.SetParent(transform);
            objects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment" && other.gameObject.tag != "Tool")
        //if (other.gameObject.tag == "Apple")
        {
            other.transform.SetParent(null);
            objects.Remove(other.gameObject);
        }
    }
}
