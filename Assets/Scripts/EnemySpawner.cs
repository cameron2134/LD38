using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

    public Text waveValueText;
    public Text enemiesRemainingText;
    public GameObject limitReachedText;

    public GameObject nextWaveText;


    private GameObject player;

    // Limit how many types of enemies can be onscreen at once so it's not too difficult
    private const int MINOR_ENEMY_LIMIT = 5,
                MAJOR_ENEMY_LIMIT = 2, 
                BOSS_ENEMY_LIMIT = 1;

    // Reset these at end of wave
    private int currentMinorEnemies = 0,
                currentMajorEnemies = 0, 
                currentBossEnemies = 0;

    private bool canSpawn;

    private const float MINOR_ENEMY_CHANCE = 0.7f,
                    MAJOR_ENEMY_CHANCE = 0.4f,
                    BOSS_ENEMY_CHANCE = 0.1f;


    private int currentEnemyCount = 0, totalEnemiesThisWave = 0;
    private int waveEnemyCount = 0;
    private int waveCount = 0;
    private bool enemyLimitReached = false;


    private void DecreaseEnemyCount() {
        currentEnemyCount--;
        enemiesRemainingText.text = currentEnemyCount.ToString();
    }


    private void IncreaseEnemyCount() {
        currentEnemyCount++;
        enemiesRemainingText.text = currentEnemyCount.ToString();
    }


    private void NextWave() {
        if (limitReachedText.activeSelf) {
            limitReachedText.GetComponent<Text>().text = "Next wave!";
            StartCoroutine(WaveTextDisappear());
        }
        Debug.Log("Next wave starting in 4 seconds!");
        totalEnemiesThisWave += 3;
        waveCount++;
        GameManager.Instance.IncreaseWavesSurvived();

        waveValueText.text = waveCount.ToString();
        InvokeRepeating("SpawnEnemy", 4f, 8f);
    }

    IEnumerator WaveTextDisappear() {
        yield return new WaitForSeconds(2f);
        limitReachedText.SetActive(false);
        limitReachedText.GetComponent<Text>().text = "Wave Limit Reached";
    }


    private void SpawnEnemy() {

        if (!enemyLimitReached) {

            float randX, randY;

            if (Random.value < 0.5) {
                randX = Random.Range(player.transform.position.x + 20, player.transform.position.x + 30);
                randY = Random.Range(player.transform.position.y + 20, player.transform.position.y + 30);
            }

            else {
                randX = Random.Range(player.transform.position.x - 20, player.transform.position.x - 30);
                randY = Random.Range(player.transform.position.y - 20, player.transform.position.y - 30);
            }


            waveEnemyCount++;

            GameObject enemy = GameManager.Instance.GetRandomPlanetObj();

            enemy.transform.position = new Vector2(randX, randY);
            enemy.SetActive(true);

            Debug.Log("New enemies spawned.");
            IncreaseEnemyCount();

            if (waveEnemyCount == totalEnemiesThisWave) {
                limitReachedText.SetActive(true);
                enemyLimitReached = true;
            }
        }

    }


    private void OnDisable() {
        GameManager.Instance.EnemyDied -= DecreaseEnemyCount;
    }


    // Use this for initialization
    void Start () {
        GameManager.Instance.EnemyDied += DecreaseEnemyCount;
        // Spawn randomly reasonable distance away from player
        player = GameObject.FindGameObjectWithTag("Player");
        NextWave();
        //InvokeRepeating("SpawnEnemy", 0, 8f);
	}
	
	// Update is called once per frame
	void Update () {

        if (enemyLimitReached) {
            CancelInvoke("SpawnEnemy");

            if (currentEnemyCount == 0) {
                waveEnemyCount = 0;
                NextWave();
                enemyLimitReached = false;
            }
        }

	}
}
