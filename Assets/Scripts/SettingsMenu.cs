using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject panel;
    public bool isPanelOpen = false;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();   
    }
    public void OpenMenu()
    {
        isPanelOpen = true;
        panel.SetActive(true);
        gameManager.PauseGame();
    }
    public void CloseMenu()
    {
        isPanelOpen = false;
        panel.SetActive(false);
        gameManager.ResumeGame();
    }

}
