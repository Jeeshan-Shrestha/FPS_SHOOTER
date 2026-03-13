using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    private float timerMinute = 5f;
    private float timerSecond = 0f;
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (timerMinute <= 0)
        {
            GameOver();
        }
        if (timerSecond < 0)
        {
            timerMinute -= 1;
            timerSecond = 60;
        }
        timerSecond -= Time.deltaTime;
        timerText.text = "Time Left:  " + timerMinute.ToString() + ":" + ((int)timerSecond).ToString();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(1);
    }

}