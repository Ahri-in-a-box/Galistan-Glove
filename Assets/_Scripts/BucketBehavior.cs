using System.Collections.Generic;
using UnityEngine;

public class BucketBehavior : MonoBehaviour
{
    private readonly List<GameObject> objects = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment" && other.gameObject.tag != "Tool")
        {
            other.transform.SetParent(transform);
            objects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Ground" && other.gameObject.tag != "Environment" && other.gameObject.tag != "Tool")
        {
            other.transform.SetParent(null);
            objects.Remove(other.gameObject);
        }
    }
}
