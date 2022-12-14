using System.Collections;
using System.Collections.Generic;
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
                    break;
                case ButtonType.STATE:
                    break;
                case ButtonType.QUIT:
                    break;
                default:
                    break;
            }
        }
    }
}
