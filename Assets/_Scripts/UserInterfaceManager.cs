using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private Button menu;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject sampleScene;
    [SerializeField] private GameObject testScene;
    [SerializeField] private GameObject galinstan;

    private bool isActive = false;

    //at start : true is test, false is sample

    private void Awake()
    {
        BluetoothHandler.OnStateChange += BTHInstance_OnStateChange;
        BTHInstance_OnStateChange();
    }

    private void BTHInstance_OnStateChange()
    {
        switch (BluetoothHandler.state)
        {
            case BluetoothHandler.BLState.Connected:
                galinstan.GetComponentInChildren<TextMeshProUGUI>().text = "Disconnect";
                galinstan.GetComponentInChildren<Button>().enabled = true;
                break;
            case BluetoothHandler.BLState.Disconnected:
                galinstan.GetComponentInChildren<TextMeshProUGUI>().text = "Connect";
                galinstan.GetComponentInChildren<Button>().enabled = true;
                break;
            case BluetoothHandler.BLState.Connecting:
                galinstan.GetComponentInChildren<TextMeshProUGUI>().text = "Connecting...";
                galinstan.GetComponentInChildren<Button>().enabled = false;
                break;
        }
    }

    private void Start()
    {

    }
    public void OnMenuPressed(GameObject panel)
    {
        panel.SetActive(isActive = !isActive);
    }

    private void OnDestroy()
    {
        print("Detroyed");
        BluetoothHandler.OnStateChange -= BTHInstance_OnStateChange;
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void SceneLoader(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ActDeactGalinstan()
    {
        switch(BluetoothHandler.state)
        {
            case BluetoothHandler.BLState.Connected:
                BluetoothHandler.DeactivateWeight();
                break;
            case BluetoothHandler.BLState.Disconnected:
                BluetoothHandler.ActivateWeight();
                break;
        }
    }
}
