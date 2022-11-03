using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private Button menu;
    [SerializeField] private GameObject panel;

    private bool isActive = false;

    public void OnMenuPressed(GameObject panel)
    {
        /*if (isActive == false)
        {
            panel.SetActive(true);
            isActive = true;
        }
        else if (isActive == true)
        {
            panel.SetActive(false);
            isActive = false;
        }*/
        panel.SetActive(isActive = !isActive);
    }
}
