using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour {

    private int playerHealth;

    public Animator anim;
    public AudioSource source;
    public AudioClip deathClip;

    public void TakeDamage(int damage) {
        if (playerHealth > 0)
            playerHealth -= damage;

        else {
            playerHealth = 0;
            // trigger an animation on death rather htan deactivate
            anim.SetBool("isDead", true);
            source.clip = deathClip;
            source.Play();
            StartCoroutine(DeathTimer());
            // Go to a new screen when dead
            //gameObject.SetActive(false);
        }
    }

    IEnumerator DeathTimer() {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        GameManager.Instance.GameOver();
    }

	// Use this for initialization
	void Start () {
        playerHealth = 100;
        GameManager.Instance.PlayerWasDamaged += TakeDamage;
	}

    private void OnDisable() {
        GameManager.Instance.PlayerWasDamaged -= TakeDamage;
    }



    // Update is called once per frame
    void Update () {
		
	}
}
