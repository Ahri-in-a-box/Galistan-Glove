using UnityEngine;

enum ButtonType
{
    NONE,
    MENU,
    OPTIONS,
    SCENE_SWAP,
    STATE,
    QUIT,
    CALIBRATE
}

public class ButtonClickable : MonoBehaviour
{
    [SerializeField] UserInterfaceManager ui;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panelLogs;
    [SerializeField]
    private ButtonType m_Type = ButtonType.NONE;

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Index")
        {
            switch (m_Type)
            {
                case ButtonType.MENU:
                    if(ui)
                        ui.OnMenuPressed(panel, panelLogs);
                    break;
                case ButtonType.SCENE_SWAP:
                    if (gameObject.name == "SceneSample")
                    {
                        if (TestBluetooth.BTHInstance.simpleMode)
                            ui.SceneLoader("SampleSceneGlove");
                        else
                            ui.SceneLoader("SampleScenePoC");
                    }
                    else if (gameObject.name == "SceneTest")
                    {
                        if (TestBluetooth.BTHInstance.simpleMode)
                            ui.SceneLoader("TestSceneGloves");
                        else
                            ui.SceneLoader("TestScenePoC");
                    }    
                    break;
                case ButtonType.STATE:
                    ui.ActDeactGalinstan();
                    break;
                case ButtonType.QUIT:
                    Application.Quit();
                    break;
                case ButtonType.CALIBRATE:
                    BluetoothHandler.SendCalibrate();
                    break;
                default:
                    break;
            }
        }
    }
}
