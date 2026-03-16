using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private float timerMinute = 0f;
    private float timerSecond = 0f;
    public TextMeshProUGUI timerText;

    public float score;
    private float scoreMultiplier = 5;
    public TextMeshProUGUI scoreText;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject sniperEnemyPrefab;
    [SerializeField] private GameObject pathPrefab;

    private bool navMeshReady = false;

    public GameObject gameOverUI;
    public bool isCursorVisible = false;

    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private ShopMenu shopMenu;

    public int killCount = 0;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI finalKillCount;

    private GameObject player;

    [Header("Spawn Settings")]
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 30f;

    private Coroutine headshotCoroutine;
    public TextMeshProUGUI headshotText;

    [Header("Wave Settings")]
    public int currentWave = 0;
    public int enemiesPerWave = 5;
    public float waveDuration = 60f;
    private float waveTimer = 0f;
    private int enemiesSpawnedThisWave = 0;
    private float enemySpawnInterval = 3f;
    private float enemySpawnTimer = 0f;
    private bool waveActive = false;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveTimerText;
    public GameObject waveAnnouncerUI;
    public TextMeshProUGUI waveAnnouncerText;

    public void ShowHeadshotIndicator()
    {
        if (headshotCoroutine != null)
            StopCoroutine(headshotCoroutine);
        headshotCoroutine = StartCoroutine(FlashHeadshotText());
    }

    private IEnumerator FlashHeadshotText()
    {
        headshotText.text = "HEADSHOT!";
        headshotText.color = Color.red;
        yield return new WaitForSeconds(1f);
        headshotText.text = "";
    }

    void Start()
    {
        Cursor.visible = isCursorVisible;
        Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = 1f;
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshReady = true;

        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;

        if (waveActive)
        {
            waveTimer -= Time.deltaTime;
            enemySpawnTimer += Time.deltaTime;

            // spawn enemies evenly throughout wave duration
            if (enemySpawnTimer >= enemySpawnInterval
                && enemiesSpawnedThisWave < enemiesPerWave
                && navMeshReady)
            {
                SpawnEnemy();
                enemiesSpawnedThisWave++;
                enemySpawnTimer = 0f;
            }

            // update wave timer UI
            int seconds = Mathf.CeilToInt(waveTimer);
            waveTimerText.text = "Wave ends in: " + seconds + "s";

            // wave over when timer runs out
            if (waveTimer <= 0f)
            {
                waveActive = false;
                StartCoroutine(StartNextWave());
            }
        }

        if (timerSecond > 60)
        {
            timerMinute += 1;
            timerSecond = 0;
        }
        timerSecond += Time.deltaTime;
        timerText.text = "Time: " + timerMinute.ToString() + ":" + ((int)timerSecond).ToString();
        scoreText.text = "Score: " + ((int)score).ToString();
        waveText.text = "Wave: " + currentWave.ToString();

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCursorVisible = !isCursorVisible;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        killCountText.text = "Kills: " + killCount.ToString();
    }

    private IEnumerator StartNextWave()
    {
        currentWave++;

        // scale difficulty each wave
        enemiesPerWave = 5 + (currentWave - 1) * 2; // wave 1 = 5, wave 2 = 7, wave 3 = 9 etc
        scoreMultiplier = 5 + currentWave * 0.5f;
        enemySpawnInterval = Mathf.Max(1f, 3f - currentWave * 0.2f); // spawns faster each wave

        // show wave announcer
        if (waveAnnouncerUI != null)
        {
            waveAnnouncerUI.SetActive(true);
            waveAnnouncerText.text = "WAVE " + currentWave;
            yield return new WaitForSeconds(2f);
            waveAnnouncerUI.SetActive(false);
        }

        // reset wave state
        enemiesSpawnedThisWave = 0;
        enemySpawnTimer = 0f;
        waveTimer = waveDuration;
        waveActive = true;
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        TextMeshProUGUI finalScore = gameOverUI.GetComponentInChildren<TextMeshProUGUI>();
        finalScore.text = "Final Score: " + ((int)score).ToString();
        finalKillCount.text = "Kills: " + killCount.ToString();
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPos = GetSpawnPositionNearPlayer();
        if (spawnPos == Vector3.zero) return;

        bool spawnSniper = sniperEnemyPrefab != null && Random.value < 0.2f;
        GameObject prefabToSpawn = spawnSniper ? sniperEnemyPrefab : enemyPrefab;

        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        GameObject enemyPath = Instantiate(pathPrefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.enemyPath = enemyPath.GetComponent<EnemyPath>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private Vector3 GetSpawnPositionNearPlayer()
    {
        if (player == null) return Vector3.zero;

        Vector3 playerPos = player.transform.position;

        for (int i = 0; i < 50; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 candidatePos = new Vector3(
                playerPos.x + Mathf.Cos(angle) * distance,
                playerPos.y + 50f,
                playerPos.z + Mathf.Sin(angle) * distance
            );

            RaycastHit groundHit;
            if (!Physics.Raycast(candidatePos, Vector3.down, out groundHit, 200f))
                continue;

            Vector3 groundPos = groundHit.point + Vector3.up * 0.5f;

            if (Physics.Raycast(groundPos + Vector3.up * 0.1f, Vector3.up, 5f))
                continue;

            NavMeshHit navHit;
            if (!NavMesh.SamplePosition(groundPos, out navHit, 3f, NavMesh.AllAreas))
                continue;

            float actualDistance = Vector3.Distance(navHit.position, playerPos);
            if (actualDistance < minSpawnDistance || actualDistance > maxSpawnDistance)
                continue;

            return navHit.position + Vector3.up * 0.5f;
        }

        NavMeshHit fallback;
        if (NavMesh.SamplePosition(playerPos + Vector3.forward * minSpawnDistance,
            out fallback, 50f, NavMesh.AllAreas))
            return fallback.position + Vector3.up * 0.5f;

        return Vector3.zero;
    }
}