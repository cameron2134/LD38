using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    private string weaponName;
    private int weaponDamage;


    public string WeaponName {
        get {
            return weaponName;
        }

        protected set {
            weaponName = value;
        }
    }


    public int WeaponDamage {
        get {
            return weaponDamage;
        }

        protected set {
            weaponDamage = value;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
