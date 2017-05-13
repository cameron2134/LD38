using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour {

    private Renderer rend;

    private void Start() {
        rend = GetComponent<Renderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        StartCoroutine(FlashDmg());
    }


    IEnumerator FlashDmg() {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        rend.material.color = Color.white;
    }
}
