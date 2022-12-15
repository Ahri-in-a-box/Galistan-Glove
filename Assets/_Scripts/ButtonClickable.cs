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
                case ButtonType.OPTIONS:
                    break;
                case ButtonType.SCENE_SWAP:
                    print(gameObject.name);
                    if (other.gameObject.name == "SceneSample")
                    {
                        print("case 1");
                        ui.SceneLoader("SampleScene");
                    }
                    else if (gameObject.name == "SceneTest")
                    {
                        ui.SceneLoader("TestScene");
                        print("case 2");
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
