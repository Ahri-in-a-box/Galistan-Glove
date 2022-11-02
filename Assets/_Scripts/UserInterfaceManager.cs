using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private Button menu;
    [SerializeField] private GameObject panel;

    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuPressed(GameObject panel)
    {
        if (isActive == false)
        {
            panel.SetActive(true);
            isActive = true;
        }
        else if (isActive == true)
        {
            panel.SetActive(false);
            isActive = false;
        }
    }
}
