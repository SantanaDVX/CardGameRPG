using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public List<Transform> spawnPoints;
    private List<int> spawnPointIndices;

    public void Awake() {
        spawnPointIndices = new List<int>();

        for (int i = 0; i < spawnPoints.Count; i++) {
            spawnPointIndices.Add(i);
        }
    }

    public List<CombatCharacter> spawnEnemies(List<GameObject> enemies) {
        if (enemies.Count > spawnPoints.Count) {
            Debug.LogError("Too many enemies; not enough spawnpoints");
        }

        shuffleSpawnPointIndecies();

        List<CombatCharacter> enemiesList = new List<CombatCharacter>();
        for (int i = 0; i < enemies.Count; i++) {
            GameObject enemy = Instantiate(enemies[i], spawnPoints[spawnPointIndices[i]].position, spawnPoints[spawnPointIndices[i]].rotation, spawnPoints[spawnPointIndices[i]]);
            enemiesList.Add(enemy.GetComponent<CombatCharacter>());
        }

        return enemiesList;
    }

    private void shuffleSpawnPointIndecies() {
        for (int i = 0; i < spawnPointIndices.Count; i++) {
            int temp = spawnPointIndices[i];
            int randomIndex = Random.Range(i, spawnPointIndices.Count);
            spawnPointIndices[i] = spawnPointIndices[randomIndex];
            spawnPointIndices[randomIndex] = temp;
        }
    }

    private static EnemySpawner enemySpawner;
    public static EnemySpawner Instance() {
        if (!enemySpawner) {
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        return enemySpawner;
    }
}
