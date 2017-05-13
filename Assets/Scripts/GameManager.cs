using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject highscorePanel;
    public Text wavesSurvivedText, enemiesKilledText;


    public static GameManager Instance = null;

    private string[] planetNamesList = { "Sobreinope", "Patreupra", "Woskippe", "Bugillon", "Yeira", "Hoinides", "Stragua CKJ4", "Koalia", "Necrolix", "Quliv", "Dryria 2QQ" };

    private int enemiesKilled, wavesSurvived;


    public void RestartGame() {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }

    public void GameOver() {
        highscorePanel.SetActive(true);
        wavesSurvivedText.text = wavesSurvived.ToString();
        enemiesKilledText.text = enemiesKilled.ToString();
    }


    public void IncreaseWavesSurvived() {
        wavesSurvived++;
    }

    public void IncreaseEnemiesKilled() {
        enemiesKilled++;
    }



    public delegate void PlayerDamaged(int damage);
    public event PlayerDamaged PlayerWasDamaged;

    public void OnPlayerDamaged(int damage) {
        if (PlayerWasDamaged != null)
            PlayerWasDamaged(damage);

        else
            Debug.Log("No subscribers to PlayerWasDamaged");


    }


    public delegate void ShieldCooldown(int cooldown);
    public event ShieldCooldown ShieldUsed;

    public void OnShieldUsed(int cooldown) {
        if (ShieldUsed != null)
            ShieldUsed(cooldown);

        else
            Debug.Log("No subscribers to ShieldUsed");
    }



    public delegate void MissileCooldown(int cooldown);
    public event MissileCooldown MissilesUsed;

    public void OnMissilesUsed (int cooldown) {

        if (MissilesUsed != null)
            MissilesUsed(cooldown);

        else
            Debug.Log("No subscribers to MissilesUsed");

    }



    public delegate void EnemyDeath();
    public event EnemyDeath EnemyDied;

    public void OnEnemyDied() {
        if (EnemyDied != null) {
            EnemyDied();
            IncreaseEnemiesKilled();
        }

        else
            Debug.Log("No subscribers to EnemyDied");
    }





    public string GetRandomPlanetName() {
        return planetNamesList[Random.Range(0, planetNamesList.Length)];
    }


    public void QuitGame() {
        Application.Quit();
    }
 

    void Awake() {
        if (Instance == null)
            Instance = this;

        //planetNamesList = new List<string>(System.IO.File.ReadAllLines(@"Assets\Data\PlanetNames.txt"));

        LoadGameData();
    }





    #region Wave Manager

    private int currentWave;

    #endregion







    #region Object Pool code

    public GameObject activeObjectFolder, inactiveObjectFolder;

    private List<GameObject> list;



    private int totalLasers = 10,
                totalEnemies = 20,
                totalEnemyLasers = 50,
                totalEnemyPulses = 50,
                totalMissiles = 30;

    public int listSize;





    /// <summary>
    /// 'Destroys' a game object by returning it to the object pool for later use.
    /// </summary>
    /// <param name="obj">The game object to destroy.</param>
    public void DestroyObj(GameObject obj) {

        obj.SetActive(false);
        obj.transform.SetParent(inactiveObjectFolder.transform);
        obj.transform.position = inactiveObjectFolder.transform.position;
        list.Add(obj);


    }




    /// <summary>
    /// Searches for the specified game object and retrieves it from the pool.
    /// </summary>
    /// <param name="obj">The game object to retrieve.</param>
    /// <returns>The specified game object.</returns>
    public GameObject GetGameObj(GameObject obj) {

        if (list.Count > 0) {

            // Finding the specified game object in the pool
            GameObject returnObj = list.Find(o => o.name == (obj.name + "(Clone)"));
            returnObj.transform.SetParent(activeObjectFolder.transform);
            list.Remove(returnObj);

            return returnObj;
        }

        return null;
    }



    /// <summary>
    /// Searches for the specified game object as a string and retrieves it from the pool.
    /// </summary>
    /// <param name="obj">The game object to retrieve.</param>
    /// <returns>The specified game object.</returns>
    public GameObject GetGameObj(string obj) {

        GameObject returnObj;


        if (list.Count > 0) {

            if (obj.Contains("(Clone)"))
                returnObj = list.Find(o => o.name == obj);


            else
                returnObj = list.Find(o => o.name == (obj + "(Clone)"));

            returnObj.transform.SetParent(activeObjectFolder.transform);
            list.Remove(returnObj);


            return returnObj;
        }

        return null;
    }



    public GameObject GetRandomPlanetObj() {
        string[] planetNames = { "Enemy Water", "Enemy Sand", "Enemy Red"};
        string obj = planetNames[Random.Range(0, planetNames.Length)];

        GameObject returnObj;


        if (list.Count > 0) {

            if (obj.Contains("(Clone)"))
                returnObj = list.Find(o => o.name == obj);


            else
                returnObj = list.Find(o => o.name == (obj + "(Clone)"));

            returnObj.transform.SetParent(activeObjectFolder.transform);
            list.Remove(returnObj);


            return returnObj;
        }

        return null;
    }




    /// <summary>
    /// Loads all of the game objects and disable them.
    /// </summary>
    public void LoadGameData() {
        list = new List<GameObject>();


        for (int x = 0; x < totalLasers; x++)
            Load("Laser");


        

        for (int x = 0; x < totalEnemies; x++) {
            float rand = Random.value;
            if (rand <= 0.5f)
                Load("Enemy Sand");
            else if (rand <= 0.7f)
                Load("Enemy Water");
            else
                Load("Enemy Red");
        }
            //Load("Enemy");

        for (int x = 0; x < totalEnemyLasers; x++)
            Load("Enemy Laser");

        for (int x = 0; x < totalEnemyPulses; x++)
            Load("Enemy Pulse");

        for (int x = 0; x < totalMissiles; x++)
            Load("Missile");
    }



    private void Load(string path) {

        GameObject objToLoad = (GameObject)Instantiate(Resources.Load(path), inactiveObjectFolder.transform.position, Quaternion.identity);
        objToLoad.transform.SetParent(inactiveObjectFolder.transform);
        objToLoad.SetActive(false);

        this.list.Add(objToLoad);

    }



    #endregion


}
