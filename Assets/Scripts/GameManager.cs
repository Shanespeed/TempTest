using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int lives = 50;
    public int currency = 100;
    public int currentWave = 0;
    public int waveCompleteBonus = 100;
    public float spawnRate = 0.2f;
    
    [HideInInspector] public List<GameObject> activeEnemies;
    private int activeEnemyCount;
    private bool waveInProgress;
    private bool isSpedUp = false;
    private Spawner spawner;
    private Button nextWaveButton;
    private TextMeshProUGUI speedUpButtonText;

    private void Awake()
    {
        spawner = GameObject.Find("EnemySpawner").GetComponent<Spawner>();
        nextWaveButton = GameObject.Find("NextWaveButton").GetComponent<Button>();
        speedUpButtonText = GameObject.Find("SpeedUpButtonText").GetComponent<TextMeshProUGUI>();
        InvokeRepeating("ActiveEnemyCheck", 0, 1f);
    }

    // Checks amount of enemies left and completes wave if none are left
    private void ActiveEnemyCheck()
    {
        activeEnemyCount = activeEnemies.Count;
        
        if (activeEnemyCount <= 0 && waveInProgress)
            WaveComplete();
    }

    // Gets called by the next wave UI button
    public void NextWave()
    {
        spawner.StartWave();
        waveInProgress = true;
        nextWaveButton.interactable = false;
    }

    // Gets called when all enemies are dead and the wave was still in progress
    private void WaveComplete()
    {
        if (currentWave >= spawner.waves.Length - 1)
        {
            Debug.Log("YOU WIN");
            nextWaveButton.interactable = false;
            waveInProgress = false;
            return;
        }

        currentWave++;
        currency += waveCompleteBonus;
        waveInProgress = false;
        nextWaveButton.interactable = true;
    }

    // Gets called by speed up UI button
    public void ChangeGameSpeed()
    {
        // Speed the game up
        if (!isSpedUp)
        {
            isSpedUp = true;
            speedUpButtonText.text = ">>";
            Time.timeScale = 2f;
        }
        else // Slow the game down
        {
            isSpedUp = false;
            speedUpButtonText.text = ">";
            Time.timeScale = 1f;
        }
    }
}
