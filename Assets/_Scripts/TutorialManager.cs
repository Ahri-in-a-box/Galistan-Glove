using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tpPanel;
    [SerializeField] private GameObject grabPanel1;
    [SerializeField] private GameObject grabPanel2;
    [SerializeField] private GameObject menuPanel;
    public enum TutorialState
    {
        TELEPORT,
        OBJECT1,
        OBJECT2,
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
            State = TutorialState.OBJECT1;
            tpPanel.SetActive(false);
            grabPanel1.SetActive(true);
        }
    }

    public void OnObjectTaken(int val)
    {
        if(State == TutorialState.OBJECT1 && val == 1)
        {
            State = TutorialState.OBJECT2;
            grabPanel1.SetActive(false);
            grabPanel2.SetActive(true);
        }

        if(State == TutorialState.OBJECT2 && val == 2)
        {
            State = TutorialState.SWAP_SCENE;
            grabPanel2.SetActive(false);
            menuPanel.SetActive(true);
        }
    }

    public void OnBluetoothChange()
    {
        if(State == TutorialState.BLUETOOTH)
        {

        }
    }
}
