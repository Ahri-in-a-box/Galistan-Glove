using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbableLeftRight : MonoBehaviour
{
    [SerializeField] private Transform rightOffset;

    
    public void OnLeftGrabEnter(SelectEnterEventArgs args)
    {
        Transform attachTransform = args.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>().attachTransform;
        attachTransform.position = rightOffset.position + attachTransform.position;
        attachTransform.rotation = rightOffset.rotation * attachTransform.rotation;

    }

    public void OnLeftGrabExit(SelectExitEventArgs args)
    {
        Transform attachTransform = args.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>().attachTransform;
        attachTransform.position = rightOffset.position - attachTransform.position;
        attachTransform.rotation = rightOffset.rotation * Quaternion.Inverse(attachTransform.rotation);
    }


}
