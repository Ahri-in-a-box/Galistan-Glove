using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    //private Vector3 mOffset;///Used with Mouse to be removed
    //private float mZCoord; ///Used with Mouse to be removed
 
    private bool active = false;
    public GameObject player;
    public GameObject Cube;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    /// <summary>
    /// Used with mouse to be removed
    /// </summary>
    public void OnGrabExited()
    {
        //mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        //// Store offset = gameobject world pos - mouse world pos
        //mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        active = true;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    /// <summary>
    /// Used with mouse to be removed
    /// </summary>
    /// <returns></returns>
    //private Vector3 GetMouseAsWorldPoint()
    //{
    //    // Pixel coordinates of mouse (x,y)
    //    Vector3 mousePoint = Input.mousePosition;

    //    // z coordinate of game object on screen
    //    mousePoint.z = mZCoord;

    //    // Convert it to world points
    //    return Camera.main.ScreenToWorldPoint(mousePoint);
    //}

    /// <summary>
    /// Used with mouse to be removed
    /// </summary>
    /// <returns></returns>
    //void OnMouseDrag()
    //{
    //    transform.position = GetMouseAsWorldPoint() + mOffset;
    //}



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
            Debug.Log("Boom");
            active = false;
            player.transform.position = position;
            Instantiate(Cube, new Vector3(position.x, 0.7f, position.z + 0.3f), rotation);
            // Instantiate(explosionPrefab, position, rotation);
            Destroy(gameObject);
        }
    }
}
