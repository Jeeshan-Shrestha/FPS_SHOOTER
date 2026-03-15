using System;
using System.Collections.Generic;
using Dan.Main;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] GameObject leaderboardPanel;
    [SerializeField]private List<TextMeshProUGUI> names;

    [SerializeField]
    private List<TextMeshProUGUI> scores;
    [SerializeField]private TextMeshProUGUI statusText;

    private string publicKey = "31da00c331cd09265dc08bd8627263fe49aeb77765b4ff1a38a17d79c232b2dc";
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        GetLeaderboard();
    }
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, (msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0 ; i < loopLength ; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        });
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        // fetch first to check existing score
        LeaderboardCreator.GetLeaderboard(publicKey, (msg) =>
        {
            LeaderboardCreator.UploadNewEntry(publicKey, username, score, (success) =>
                {
                    if (success)
                    {
                        statusText.text = "Successfully Updated";
                        GetLeaderboard();
                    }
                    else
                        statusText.text = "Upload failed";
                });
        });
    }

    public void OpenLeaderboard()
    {
        GetLeaderboard();
        leaderboardPanel.SetActive(true);
        gameManager.PauseGame();
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        gameManager.ResumeGame();
    }
}
