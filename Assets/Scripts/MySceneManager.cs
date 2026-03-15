using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{

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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
