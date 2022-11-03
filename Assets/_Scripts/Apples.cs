using UnityEngine;

public class Apples : MonoBehaviour
{
    public void OnPicked()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}