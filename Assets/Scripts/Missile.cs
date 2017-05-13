using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon {

    private bool isSeeking = false;

    private Vector3 pos;

	// Use this for initialization
	void Start () {
        WeaponDamage = 20;
	}
	
	// Update is called once per frame
	void FixedUpdate () {


            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 100);

            int i = 0;
            while (i < hitColliders.Length) {

                if (hitColliders[i].tag == "Enemy") {
                    pos = hitColliders[i].transform.position;

                    transform.position = Vector2.MoveTowards(transform.position, pos, 8 * Time.deltaTime);

                    Vector3 diff = pos - transform.position;

                    float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), 8 * Time.deltaTime);

                    isSeeking = true;
                    break;
                }

                i++;
            }
        

    }


    private void Update() {
      
    }


    private void FlyToEnemy(Vector3 enemyPos) {

        

    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy")
            GameManager.Instance.DestroyObj(gameObject);
    }

}
