using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIHolder : MonoBehaviour {

    public Text nameText, healthText;
    //public Slider healthBar;
    public Enemy enemyObj;
    public Animator anim;


    public void TakeDamage(int damage) {

        if (enemyObj.EntityHealth <= 0) {
            
            healthText.text = "0";
            anim.SetBool("isDead", true);
            StartCoroutine(DeathTimer());
        }

        else {
            healthText.text = (Int32.Parse(healthText.text) - damage).ToString();
        }

    }


    IEnumerator DeathTimer() {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.OnEnemyDied();
        Die();
    }

    // Use this for initialization
    void Start() {

        //Entity entity = transform.parent.GetComponent<Enemy>();

        healthText.text = enemyObj.EntityHealth.ToString();
        nameText.text = enemyObj.EntityName;
        


    }



    private void Die() {
        GameManager.Instance.DestroyObj(enemyObj.gameObject);
    }

}
