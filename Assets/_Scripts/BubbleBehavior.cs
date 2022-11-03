using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private Vector3 pos;

    // Update is called once per frame
    void Update()
    {
        pos = playerCamera.transform.position - transform.position;
        pos.x = 0.0f;
        pos.z = 0.0f;

        transform.LookAt(playerCamera.transform.position - pos);
    }
}
