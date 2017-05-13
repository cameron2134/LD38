using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    private string entityName;
    private int entityHealth, entitySpeed;




    public string EntityName {
        get {
            return entityName;
        }

        protected set {
            entityName = value;
        }
    }


    public int EntityHealth {
        get {
            return entityHealth;
        }

        protected set {
            entityHealth = value;
        }
    }


    public int EntitySpeed {
        get {
            return entitySpeed;
        }

        protected set {
            entitySpeed = value;
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
