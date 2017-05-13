using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon {

	// Use this for initialization
	void Start () {
        WeaponName = "Laser";
        WeaponDamage = 15;
	}
	


    private void OnCollisionEnter2D(Collision2D collision) {

        
        GameManager.Instance.DestroyObj(gameObject);

    }




}
