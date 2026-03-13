using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Enter the game");
        SceneManager.LoadScene(1);
    }
    public void LeaveGame()
    {
        Debug.Log("Exited the game");
        Application.Quit();
    }
}
