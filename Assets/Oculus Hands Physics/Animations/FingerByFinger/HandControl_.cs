using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControl_ : MonoBehaviour
{

    private ActionBasedController controller;
    public Hand_ hand;

    [SerializeField] private HandGetter handParent;

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

        hand.SetIndex(handParent.Curls[1]);
        hand.SetMiddle(handParent.Curls[2]);
        hand.SetRing(handParent.Curls[3]);
        hand.SetPinky(handParent.Curls[4]);
        hand.SetThumb(handParent.Curls[0]);
        //print(handParent.Curls[0]);
    }
    
}
