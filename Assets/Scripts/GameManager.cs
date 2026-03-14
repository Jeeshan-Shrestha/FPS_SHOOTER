using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;
using Unity.VisualScripting;

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
    [SerializeField] private NavMeshSurface navMeshSurface;

    private Vector3 mapCenter;
    private bool navMeshReady = false;

    private float enemySpawnCooldown = 10f;
    private float enemySpawnTimer;

    public GameObject gameOverUI;

    public bool isCursorVisible = false;

    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private ShopMenu shopMenu;

    public int killCount = 0;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI finalKillCount;

    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(BuildNavMeshAndReady());
    }

    private IEnumerator BuildNavMeshAndReady()
    {
        navMeshSurface.BuildNavMesh();
        yield return new WaitForSeconds(0.5f);

        // Automatically calculate real center from NavMesh
        NavMeshTriangulation tri = NavMesh.CalculateTriangulation();
        if (tri.vertices.Length > 0)
        {
            Vector3 sum = Vector3.zero;
            foreach (Vector3 v in tri.vertices) sum += v;
            mapCenter = sum / tri.vertices.Length;
            Debug.Log($"NavMesh ready! Center: {mapCenter}");
            navMeshReady = true;
        }
        else
        {
            Debug.LogError("NavMesh has no vertices! Check NavMeshSurface is assigned and baked.");
        }
    }

    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer > enemySpawnCooldown && navMeshReady)
        {
            SpawnEnemy();
        }
        if (timerSecond > 60)
        {
            timerMinute += 1;
            timerSecond = 0;
        }
        timerSecond += Time.deltaTime;
        timerText.text = "Time: " + timerMinute.ToString() + ":" + ((int)timerSecond).ToString();
        scoreText.text = "Score: " + ((int)score).ToString();

        if (Input.GetKeyDown(KeyCode.LeftControl))
    {
        isCursorVisible = !isCursorVisible;

        Cursor.visible = isCursorVisible;

        if (isCursorVisible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

        killCountText.text = "Kills: " + killCount.ToString();
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
        enemySpawnTimer = 0;
        enemySpawnCooldown -= 0.1f;
        scoreMultiplier += 0.01f;
        if (enemySpawnCooldown <= 1)
            enemySpawnCooldown = 1;

        Vector3 spawnPos = GetRandomNavMeshPosition(mapCenter, 100f);

        // 20% chance to spawn sniper enemy
        bool spawnSniper = sniperEnemyPrefab != null && Random.value < 0.2f;
        GameObject prefabToSpawn = spawnSniper ? sniperEnemyPrefab : enemyPrefab;

        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        GameObject enemyPath = Instantiate(pathPrefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.enemyPath = enemyPath.GetComponent<EnemyPath>();
    }

    public Vector3 GetRandomNavMeshPosition(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * range;
            Vector3 randomPos = new Vector3(
                center.x + randomCircle.x,
                center.y,
                center.z + randomCircle.y
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 50f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // Fallback: return any valid NavMesh point
        NavMeshHit fallback;
        if (NavMesh.SamplePosition(center, out fallback, 500f, NavMesh.AllAreas))
            return fallback.position;

        Debug.LogError("No NavMesh found!");
        return center;
    }
}