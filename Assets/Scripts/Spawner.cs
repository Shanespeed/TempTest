using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int enemyToSpawn = 0;
    private GameManager gameManager;
    public Rigidbody rb;

    // A compact class that holds a GameObject array
    [Serializable] public class Wave
    { public GameObject[] enemies; }

    // An array of GameObject arrays
    public Wave[] waves;

    // Start is called before the first frame update
    void Awake() =>
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    public void StartWave() =>
        InvokeRepeating("SpawnEnemy", 0f, gameManager.spawnRate);

    // Spawns enemies in the current wave in the assigned order
    private void SpawnEnemy()
    {
        // Spawns enemy at the position of the spawner 
        Instantiate(waves[gameManager.currentWave].enemies[enemyToSpawn], gameObject.transform.position, transform.rotation);
        enemyToSpawn++;

        // If the last enemy has been spawned, stop the spawner and reset values
        if (enemyToSpawn > waves[gameManager.currentWave].enemies.Length - 1)
        {
            enemyToSpawn = 0;
            CancelInvoke("SpawnEnemy");
        }
    }
}
