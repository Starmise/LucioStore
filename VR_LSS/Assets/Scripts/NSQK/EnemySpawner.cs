using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawning Settings")]
    [Tooltip("Prefab del enemigo que se va a spawnear.")]
    public GameObject enemyPrefab;

    [Tooltip("Puntos donde se spawnear�n los enemigos.")]
    public Transform[] spawnPoints;

    [Tooltip("Tiempo entre cada spawn de enemigos.")]
    public float spawnInterval = 3f;

    [Tooltip("Cantidad m�xima de enemigos permitidos en la escena.")]
    public int maxEnemies = 10;

    private int currentEnemyCount = 0;

    private void Start()
    {
        // Inicia el ciclo de spawneo
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies)
            return;

        // Selecciona un punto de spawn aleatorio
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawnea el enemigo en la posici�n y rotaci�n del punto de spawn
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemyCount++;
    }

    public void DecreaseEnemyCount()
    {
        // M�todo para reducir el contador de enemigos cuando uno es eliminado
        if (currentEnemyCount > 0)
            currentEnemyCount--;
    }
}
