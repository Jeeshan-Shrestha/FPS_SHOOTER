using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{

    public TMP_InputField nameInput;

    public void SaveName()
    {
        if (nameInput.text.Length > 0)
            PlayerPrefs.SetString("PlayerName", nameInput.text);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Enter the game");
        SceneManager.LoadScene(1);
    }

    public void LoadMap2()
    {
        SceneManager.LoadScene(1);
    }
    public void LeaveGame()
    {
        Debug.Log("Exited the game");
        Application.Quit();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
