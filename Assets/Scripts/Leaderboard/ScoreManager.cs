using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private TMP_InputField inputUsername;


    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    public UnityEvent<string,int> submitScoreEvent;
    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputUsername.text,gameManager.killCount); 
    }

}
