using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Image progressBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI statusText;

    public float minimumLoadTime = 3f; // minimum seconds to show loading screen

    private string[] loadingMessages = {
        "INITIALIZING COMBAT SYSTEMS",
        "SPAWNING ENEMIES",
        "LOADING MAP DATA",
        "PREPARING WEAPONS",
        "SYNCING NAVMESH"
    };

    void Start()
    {
        StartCoroutine(LoadSceneAsync("FloodedGround"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsed = 0f;
        float displayProgress = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;

            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // combine real progress with time progress so bar never jumps
            float timeProgress = Mathf.Clamp01(elapsed / minimumLoadTime);
            float targetProgress = Mathf.Min(realProgress, timeProgress);

            displayProgress = Mathf.MoveTowards(displayProgress, targetProgress, Time.deltaTime * 0.3f);

            progressBar.fillAmount = displayProgress;
            progressText.text = Mathf.RoundToInt(displayProgress * 100f) + "%";
            statusText.text = loadingMessages[Mathf.FloorToInt(displayProgress * (loadingMessages.Length - 1))];

            // only activate when both real load and minimum time are done
            if (operation.progress >= 0.9f && elapsed >= minimumLoadTime)
            {
                displayProgress = Mathf.MoveTowards(displayProgress, 1f, Time.deltaTime * 0.8f);
                progressBar.fillAmount = displayProgress;
                progressText.text = "100%";
                statusText.text = "READY";

                if (displayProgress >= 1f)
                {
                    yield return new WaitForSeconds(0.5f); // brief pause at 100%
                    operation.allowSceneActivation = true;
                    break;
                }
            }

            yield return null;
        }
    }
}