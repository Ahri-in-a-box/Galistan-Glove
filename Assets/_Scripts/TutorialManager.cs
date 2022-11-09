using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState
    {
        TELEPORT,
        OBJECT,
        BLUETOOTH,
        SWAP_SCENE
    }

    static public TutorialManager Instance { get; private set; }
    public TutorialState State { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        State = TutorialState.TELEPORT;

        //Events here
        BluetoothHandler.OnStateChange += OnBluetoothChange;
    }

    public void OnTeleportationInArea()
    {
        if(State == TutorialState.TELEPORT)
        {
            State = TutorialState.OBJECT;
            //Apply UI and object modifications
        }
    }

    public void OnObjectTaken(SelectEnterEventArgs args)
    {
        if(State == TutorialState.OBJECT)
        {

        }
    }

    public void OnBluetoothChange()
    {
        if(State == TutorialState.BLUETOOTH)
        {

        }
    }
}
