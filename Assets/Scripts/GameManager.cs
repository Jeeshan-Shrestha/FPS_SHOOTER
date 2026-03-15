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

    private GameObject player;

    [Header("Spawn Settings")]
    public float minSpawnDistance = 50f;
    public float maxSpawnDistance = 100f;

    void Start()
    {
        Time.timeScale = 1f;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(BuildNavMeshAndReady());
    }

    private IEnumerator BuildNavMeshAndReady()
    {
        navMeshSurface.BuildNavMesh();
        yield return new WaitForSeconds(0.5f);

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
            SpawnEnemy();

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
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
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

        Vector3 spawnPos = GetSpawnPositionNearPlayer();
        if (spawnPos == Vector3.zero) return;

        bool spawnSniper = sniperEnemyPrefab != null && Random.value < 0.2f;
        GameObject prefabToSpawn = spawnSniper ? sniperEnemyPrefab : enemyPrefab;

        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        GameObject enemyPath = Instantiate(pathPrefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.enemyPath = enemyPath.GetComponent<EnemyPath>();
    }

    private Vector3 GetSpawnPositionNearPlayer()
    {
        if (player == null) return Vector3.zero;

        Vector3 playerPos = player.transform.position;

        for (int i = 0; i < 50; i++)
        {
            // pick a random angle and random distance between min and max
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 candidatePos = new Vector3(
                playerPos.x + Mathf.Cos(angle) * distance,
                playerPos.y + 50f,
                playerPos.z + Mathf.Sin(angle) * distance
            );

            // raycast down to find ground surface
            RaycastHit groundHit;
            if (!Physics.Raycast(candidatePos, Vector3.down, out groundHit, 200f))
                continue;

            Vector3 groundPos = groundHit.point + Vector3.up * 0.5f;

            // skip if something is above — inside a building
            if (Physics.Raycast(groundPos + Vector3.up * 0.1f, Vector3.up, 5f))
                continue;

            // verify it lands on navmesh
            NavMeshHit navHit;
            if (!NavMesh.SamplePosition(groundPos, out navHit, 3f, NavMesh.AllAreas))
                continue;

            // final distance check — make sure it's actually within range
            float actualDistance = Vector3.Distance(navHit.position, playerPos);
            if (actualDistance < minSpawnDistance || actualDistance > maxSpawnDistance)
                continue;

            return navHit.position + Vector3.up * 0.5f;
        }

        // fallback — spawn anywhere valid on navmesh near player
        Debug.LogWarning("Could not find ideal spawn, using fallback");
        NavMeshHit fallback;
        if (NavMesh.SamplePosition(playerPos + Vector3.forward * minSpawnDistance, out fallback, 50f, NavMesh.AllAreas))
            return fallback.position + Vector3.up * 0.5f;

        return Vector3.zero;
    }
}