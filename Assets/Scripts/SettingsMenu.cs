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

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OpenMenu();
        }
    }
    public void OpenMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPanelOpen = true;
        panel.SetActive(true);
        gameManager.PauseGame();
    }
    public void CloseMenu()
    {
        Cursor.visible = false;
        isPanelOpen = false;
        panel.SetActive(false);
        gameManager.ResumeGame();
    }

}
