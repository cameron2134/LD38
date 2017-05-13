using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public float weaponRecoil;


    public SpriteRenderer spriteRend;
    public EnemyUIHolder uiHolder;


    public Sprite[] sprites;

    private GameObject player;
    private bool canFire, usingLongRange, usingShortRange;

    private int enemyScale;
    bool canFirePulse = true;



    void Start () {

        //Instantiate(uiHolder, transform.position, Quaternion.identity);

        enemyScale = Random.Range(2, 8);
        transform.localScale = new Vector3(enemyScale, enemyScale, 1);

        EntityName = GameManager.Instance.GetRandomPlanetName();
        EntityHealth = Random.Range(enemyScale * 20, enemyScale * 50);
        EntitySpeed = Random.Range(1, 10);

        //spriteRend.sprite = sprites[Random.Range(0, sprites.Length)];

        canFire = usingLongRange = true;
        usingShortRange = false;

        player = GameObject.FindGameObjectWithTag("Player");
	}
	

	void Update () {

        // When close to player, orbit and use a rapid firing pulse gun



        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist <= 12) {
            usingLongRange = false;
            usingShortRange = true;
            transform.RotateAround(player.transform.position, new Vector3(0, 0, 1), EntitySpeed * Time.deltaTime);
        }

        else {
            usingLongRange = true;
            usingShortRange = false;
             //Fly towards player, then within a certain range, start to orbit around them
            float step = EntitySpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

        

        
    }


    void FixedUpdate() {

        if (canFire && usingLongRange) {
            int lasersToFire;
            // Small enemies only fire 1 laser


            if (enemyScale <= 4)
                lasersToFire = 1;

            else if (enemyScale <= 6)
                lasersToFire = 2;

            else
                lasersToFire = 3;


            // Different kinds of enemies will fire differently

            Vector3 mousePos = player.transform.position;
            Vector2 mouseDir = (mousePos - transform.position);
            mouseDir = mouseDir.normalized;

            GameObject[] lasers = new GameObject[lasersToFire];

            for (int i = 0; i < lasersToFire; i++) {
                lasers[i] = GameManager.Instance.GetGameObj("Enemy Laser");

                Physics2D.IgnoreCollision(lasers[i].GetComponent<Collider2D>(), GetComponent<Collider2D>());

                lasers[i].SetActive(true);
                if (i != 0)
                    lasers[i].transform.position = new Vector2(lasers[i-1].transform.position.x + 1.5f, transform.position.y);

                else
                    lasers[i].transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y);


                lasers[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg - 90);

                lasers[i].GetComponent<Rigidbody2D>().AddForce((mouseDir * 100));

            }

            canFire = false;
            StartCoroutine(LaserRecoil());
        }


        if (canFirePulse && usingShortRange) {

            int pulsesToFire;
            // Small enemies only fire 1 laser


            if (enemyScale <= 4)
                pulsesToFire = 3;

            else if (enemyScale <= 6)
                pulsesToFire = 5;

            else
                pulsesToFire = 7;


            // Different kinds of enemies will fire differently

            Vector3 mousePos = player.transform.position;
            Vector2 mouseDir = (mousePos - transform.position);
            mouseDir = mouseDir.normalized;

            mouseDir.x += Random.Range(-0.5f, 0.5f);
            mouseDir.y += Random.Range(-0.2f, 0.2f);
            GameObject pulse;


            // Instead of amount fired, it should be speed
            if (canFirePulse) {

                pulse = GameManager.Instance.GetGameObj("Enemy Pulse");

                Physics2D.IgnoreCollision(pulse.GetComponent<Collider2D>(), GetComponent<Collider2D>());

                pulse.SetActive(true);

          
                pulse.transform.position = new Vector2(transform.position.x, transform.position.y);


                pulse.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90));

                pulse.GetComponent<Rigidbody2D>().AddForce((mouseDir * 150));

                canFirePulse = false;
                StartCoroutine(PulseRecoil());
            }

            


        }

    }


    IEnumerator PulseRecoil() {
        yield return new WaitForSeconds(0.3f);
        canFirePulse = true;
    }


    IEnumerator LaserRecoil() {
        yield return new WaitForSeconds(weaponRecoil);
        canFire = true;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Projectile") {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();

            if (EntityHealth > 0)
                EntityHealth -= weapon.WeaponDamage;
            else if (EntityHealth <= 0) {
                EntityHealth = 0;
                
            }

            uiHolder.TakeDamage(weapon.WeaponDamage);
        }
    }

}
