using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Weapon {

    

    // Asteroid can be used like a projectile

	// Use this for initialization
	void Start () {
        WeaponDamage = 50;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Asteroids just bounce around continuously causing damage to anything they hit
    private void OnCollisionEnter2D(Collision2D collision) {
        //Destroy(gameObject);
    }
}
