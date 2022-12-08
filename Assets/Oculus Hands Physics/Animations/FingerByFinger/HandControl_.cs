using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControl_ : MonoBehaviour
{

    private ActionBasedController controller;
    public Hand_ hand;

    [Range(0.0f, 1.0f)]
    public float curl1;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {

        //hand.SetIndex(controller.activateAction.action.ReadValue<float>());
        //hand.SetMiddle(controller.selectAction.action.ReadValue<float>());
        //hand.SetRing(controller.selectAction.action.ReadValue<float>());
        //hand.SetThumb(controller.selectAction.action.ReadValue<float>());
        //hand.SetPinky(controller.selectAction.action.ReadValue<float>());

        hand.SetIndex(curl1);
    }


    private void OnTriggerStay(Collider other)
    {
        if(curl1 > 0.4f && other.gameObject.GetComponent<XRGrabInteractable>().isActiveAndEnabled)
        {
            GetComponent<XRDirectInteractor>().StartManualInteraction(other.GetComponent<IXRSelectInteractable>());
        }
        else 
        { 
            GetComponent<XRDirectInteractor>().EndManualInteraction();            
        }
    }
}
