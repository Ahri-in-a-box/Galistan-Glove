using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private UnityEngine.UI.Button btn;

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
                status.text = "Disconnect";
                btn.enabled = true;
                break;
            case BluetoothHandler.BLState.Disconnected:
                status.text = "Connect";
                btn.enabled = true;
                break;
            case BluetoothHandler.BLState.Connecting:
                status.text = "Connecting...";
                btn.enabled = false;
                break;
        }
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
