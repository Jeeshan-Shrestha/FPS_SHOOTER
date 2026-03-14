using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject panel;
    public bool isPanelOpen = false;
    
    public void OpenMenu()
    {
        isPanelOpen = true;
        panel.SetActive(true);
    }
    public void CloseMenu()
    {
        isPanelOpen = false;
        panel.SetActive(false);
    }

}
