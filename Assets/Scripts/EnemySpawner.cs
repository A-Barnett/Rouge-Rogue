using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPref;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject player;
    private float enemySpeed = 1f;
    private float startEnemySpeed;
    private float spawnTimer;
    private int enemyHealth = 50;
    private float XPAmount = 10;
    private float startXPAmount;
    [SerializeField] private int spawnAmount;
    private int startSpawnAmount;
    private int startEnemyHealth;

    private void Start()
    {
        startEnemySpeed = enemySpeed;
        startXPAmount = XPAmount;
        startEnemyHealth = enemyHealth;
        startSpawnAmount = spawnAmount;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnDelay)
        {
            SpawnEnemy();
            spawnTimer = 0;
        }
    }

    public void IncreaseAmount(int level)
    {
        spawnAmount = startSpawnAmount + (int)Mathf.Pow(level,1.6f);
        enemySpeed += 0.05f;
        startEnemySpeed += 0.05f;
        enemyHealth = startEnemyHealth + (int)Mathf.Pow(level, 2);
    }

    private void SpawnEnemy()
    {
        Vector3 startSpawnPoint = SpawnPoint();
        bool groupSpawn = Random.Range(0, 2) == 0;
        for (int i = spawnAmount; i > 0; i--)
        {
            GameObject enemy;
            if (groupSpawn)
            {
                enemy = Instantiate(enemyPref, AroundSpawnPoint(startSpawnPoint), enemyPref.transform.rotation);
            }
            else
            {
                enemy = Instantiate(enemyPref, SpawnPoint(), enemyPref.transform.rotation);
            }
            enemy.transform.SetParent(gameObject.transform);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.SetSpeed(enemySpeed);
            enemyController.SetHealth(enemyHealth);
            enemyController.SetXpAmount(XPAmount);
        }
    }

    private Vector3 SpawnPoint()
    {
        Vector3 playerPos = player.transform.position;
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0:
                return playerPos + new Vector3(-13, Random.Range(-500, 500) / 100f, 0);
            case 1:
                return playerPos + new Vector3(13, Random.Range(-500, 500) / 100f, 0);
            case 2:
                return playerPos + new Vector3(Random.Range(-900, 900) / 100f, 8, 0);
            case 3:
                return playerPos + new Vector3(Random.Range(-900, 900) / 100f, -8, 0);
        }

        return Vector3.zero;
    }

    private Vector3 AroundSpawnPoint(Vector3 start)
    {
        return start + new Vector3(Random.Range(-500, 500)/300f, Random.Range(-500, 500)/300f, 0);
    }

    private void OnEnable()
    {
        UpgradeController.OnUpgradeChosen += HandleUpgradeChosen;
    }

    private void OnDisable()
    {
        UpgradeController.OnUpgradeChosen -= HandleUpgradeChosen;
    }

    private void HandleUpgradeChosen(UpgradeController.Upgrades upgrades)
    {
        enemySpeed = startEnemySpeed * Mathf.Pow((1 / 1.15f), upgrades.UpEnemySpeed);
        XPAmount = startXPAmount * Mathf.Pow((1 + 0.25f), upgrades.UpXpGain);
    }
}