using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Entity {

    public Rigidbody2D playerBody;
    public GameObject playerShield;

    public Image shieldAbilityIcon, missileAbilityIcon, superAbilityIcon;

    public float moveSpeed;


    private float moveX, moveY;
    private bool canFire, canBeDamaged;


    private const int SHIELD_DURATION = 3;

    private const int SHIELD_COOLDOWN = 10,
                        MISSILES_COOLDOWN = 15,
                        SUPER_COOLDOWN = 45;

    private bool shieldUsable, missilesUsable, superUsable;

    private bool shieldActive;

    private int missilesLaunched;
    int missilesToLaunch = 4;

    public AudioSource source;
    public AudioClip shootClip;
    public AudioClip missileClip;
    public AudioClip shieldClip;

    void Start () {
        EntityName = "Earth";
        canFire = canBeDamaged = shieldUsable = missilesUsable = true;
    }
	


    private void LaunchMissile() {

        if (missilesLaunched < missilesToLaunch) {
            GameObject missile = GameManager.Instance.GetGameObj("Missile");
            source.clip = missileClip;
            source.Play();

            Physics2D.IgnoreCollision(missile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            missile.SetActive(true);
            missile.transform.position = transform.position;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - transform.position);
            mouseDir = mouseDir.normalized;

            missile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg - 90);

            missilesLaunched++;
        }

        else {
            CancelInvoke("LaunchMissile");
            missilesUsable = false;
            StartCoroutine(MissileCooldownTimer());
            missilesLaunched = 0;
        }
    }

	void Update () {

        #region Player Movement
        moveX = moveY = 0;
        //playerAnim.SetBool("Moving", false);

        // Movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveX = -1;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveX = 1;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveY = 1;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveY = -1;
        }

        #endregion



        if (Input.GetKeyDown(KeyCode.Alpha1) && missilesUsable) {


            InvokeRepeating("LaunchMissile", 0, 0.2f);
            missileAbilityIcon.color = Color.gray;

        }


        if (Input.GetKeyDown(KeyCode.Alpha2) && shieldUsable) {
            playerShield.SetActive(true);
            shieldActive = true;
            shieldUsable = canFire = false;

            source.clip = shieldClip;
            source.Play();
            StartCoroutine(ShieldDurationTimer());
            shieldAbilityIcon.color = Color.gray;
        }
        

    }



    IEnumerator MissileCooldownTimer() {
        GameManager.Instance.OnMissilesUsed(MISSILES_COOLDOWN);
        yield return new WaitForSeconds(MISSILES_COOLDOWN);
        
        missileAbilityIcon.color = Color.white;
        missilesUsable = true;
    }


    IEnumerator ShieldDurationTimer() {
        yield return new WaitForSeconds(SHIELD_DURATION);
        playerShield.SetActive(false);
        canFire = true;
        shieldActive = false;
        GameManager.Instance.OnShieldUsed(SHIELD_COOLDOWN);
        StartCoroutine(ShieldCooldownTimer());
    }


    IEnumerator ShieldCooldownTimer() {
        
        yield return new WaitForSeconds(SHIELD_COOLDOWN);
        shieldAbilityIcon.color = Color.white;
        shieldUsable = true;
    }


    void FixedUpdate() {
        //playerBody.velocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
        playerBody.AddForce(new Vector2(moveX * moveSpeed, moveY * moveSpeed));

        if (Input.GetMouseButtonDown(0) && canFire) {
            source.clip = shootClip;
            source.Play();
            // Spawn laser and moves towards point clicked on screen
            GameObject laser = GameManager.Instance.GetGameObj("Laser");

            Physics2D.IgnoreCollision(laser.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            laser.SetActive(true);
            laser.transform.position = transform.position;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - transform.position);
            mouseDir = mouseDir.normalized;

            laser.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg - 90);


            laser.GetComponent<Rigidbody2D>().AddForce((mouseDir * 1000));
            canFire = false;
            StartCoroutine(LaserRecoil());
        }
    }



    IEnumerator LaserRecoil() {
        yield return new WaitForSeconds(0.6f);
        canFire = true;
    }



    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Projectile" && !shieldActive) {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            Debug.Log("taking " + weapon.WeaponDamage + " damage");
            GameManager.Instance.OnPlayerDamaged(weapon.WeaponDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Gas Cloud" && canBeDamaged) {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            Debug.Log("taking " + weapon.WeaponDamage + " damage");
            GameManager.Instance.OnPlayerDamaged(weapon.WeaponDamage);
            canBeDamaged = false;
            StartCoroutine(DamageOverTimeCooldown());
        }
    }


    IEnumerator DamageOverTimeCooldown() {
        yield return new WaitForSeconds(0.5f);
        canBeDamaged = true;
    }


}
