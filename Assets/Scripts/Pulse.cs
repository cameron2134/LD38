using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : Weapon {

    void Start() {
        WeaponName = "Pulse";
        WeaponDamage = 8;
    }



    private void OnCollisionEnter2D(Collision2D collision) {
        GameManager.Instance.DestroyObj(gameObject);
    }
}
