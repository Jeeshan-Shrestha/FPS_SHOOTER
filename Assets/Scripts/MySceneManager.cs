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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LeaveGame()
    {
        Debug.Log("Exited the game");
        Application.Quit();
    }
}
