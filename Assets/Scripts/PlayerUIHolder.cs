using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHolder : MonoBehaviour {

    public Text nameText;
    public Slider healthBar;

    public GameObject shieldTimer, missilesTimer;

    private Text shieldTimerText, missilesTimerText;

    private float shieldCountdown, missilesCountdown;

    //private bool timerEnabled;


    public void UpdateHealthBar(int value) {
        healthBar.value -= value;
    }



    public void ShowShieldTimer(int cooldown) {
        shieldCountdown = cooldown;

        shieldTimer.SetActive(true);
    }


    public void ShowMissilesTimer(int cooldown) {
        missilesCountdown = cooldown;

        missilesTimer.SetActive(true);
    }



    private void Update() {
        if (shieldTimer.activeSelf && shieldCountdown > 0) {
            shieldCountdown -= Time.deltaTime;
            int time = (int)shieldCountdown;

            shieldTimerText.text = time.ToString();


        }

        else {
            shieldTimer.SetActive(false);
        }


        if (missilesTimer.activeSelf && missilesCountdown > 0) {
            missilesCountdown -= Time.deltaTime;
            int time2 = (int)missilesCountdown;

          
            missilesTimerText.text = time2.ToString();
            
        }

        else {
            missilesTimer.SetActive(false);
        }

        
    }


    private void OnDisable() {
        GameManager.Instance.PlayerWasDamaged -= UpdateHealthBar;
        GameManager.Instance.ShieldUsed -= ShowShieldTimer;
        GameManager.Instance.MissilesUsed -= ShowMissilesTimer;
    }

    // Use this for initialization
    void Start () {
        GameManager.Instance.PlayerWasDamaged += UpdateHealthBar;
        GameManager.Instance.ShieldUsed += ShowShieldTimer;
        GameManager.Instance.MissilesUsed += ShowMissilesTimer;
        //nameText.text = transform.parent.GetComponent<PlayerController>().EntityName;

        shieldTimerText = shieldTimer.GetComponent<Text>();
        missilesTimerText = missilesTimer.GetComponent<Text>();
    }

}
