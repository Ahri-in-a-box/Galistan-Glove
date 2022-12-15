using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Diagnostics;

public class Teleporter : MonoBehaviour
{
    //private Vector3 mOffset;///Used with Mouse to be removed
    //private float mZCoord; ///Used with Mouse to be removed
 
    private bool active = false;
    public GameObject player;
    public GameObject Cube;
    public Rigidbody rb;
    public XRSocketInteractor attach;

    private Stopwatch timer = new();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        attach.StartManualInteraction(Cube.GetComponent<IXRSelectInteractable>());
    }

    /// <summary>
    /// Used with mouse to be removed
    /// </summary>
    
    public void OnGrabEnter()
    {
        timer.Reset();
    }
    public void OnGrabExited()
    {
        active = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        timer.Start();

    }

    private void FixedUpdate()
    {
        if(timer.ElapsedMilliseconds >= 5000.0f)
        {
            attach.StartManualInteraction(Cube.GetComponent<IXRSelectInteractable>());
            timer.Reset();
        }
    }
  

    void OnCollisionEnter(Collision collision)
    {
        if (!active)
        {
            return;
        }
        if(collision.gameObject.tag == "Ground")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            active = false;
            player.transform.position = position;

            attach.StartManualInteraction(Cube.GetComponent<IXRSelectInteractable>());

            collision.gameObject.GetComponent<TeleportationArea>()?.teleporting.Invoke(new TeleportingEventArgs());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Boundary")
        {
            attach.StartManualInteraction(Cube.GetComponent<IXRSelectInteractable>());
        }
    }
}
