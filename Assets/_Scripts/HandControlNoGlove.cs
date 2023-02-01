using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControlNoGlove : MonoBehaviour
{
    ActionBasedController controller;

    public Hand_ hand;
    public Hand_ handPhysics;

    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }
    void Update()
    {
        hand.SetPoint(controller.activateAction.action.ReadValue<float>());
        handPhysics.SetPoint(controller.activateAction.action.ReadValue<float>());
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        handPhysics.SetGrip(controller.selectAction.action.ReadValue<float>());
    }
}
