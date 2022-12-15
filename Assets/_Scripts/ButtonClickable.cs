using UnityEngine;

enum ButtonType
{
    NONE,
    MENU,
    OPTIONS,
    SCENE_SWAP,
    STATE,
    QUIT
}

public class ButtonClickable : MonoBehaviour
{
    [SerializeField] UserInterfaceManager ui;
    [SerializeField] GameObject panel;
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
                        ui.OnMenuPressed(panel);
                    break;
                case ButtonType.SCENE_SWAP:
                    if (other.gameObject.name == "SceneSample")
                    {
                        ui.SceneLoader("SampleScene");
                    }
                    else if (gameObject.name == "SceneTest")
                    {
                        if(TestBluetooth.BTHInstance.simpleMode)
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
                default:
                    break;
            }
        }
    }
}
