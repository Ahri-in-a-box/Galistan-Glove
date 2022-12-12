using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickable : MonoBehaviour
{

    [SerializeField] UserInterfaceManager ui;
    [SerializeField] GameObject panel;
    private UnityEngine.UI.Button btn;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<UnityEngine.UI.Button>();    
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Index")
        {
            ui.OnMenuPressed(panel);
        }
    }
}
