using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {
    [SerializeField] int timeToExplosion = 2;
    [SerializeField] int radioExplosion = 3;
    [SerializeField] int forceExplosion = 100;
    [SerializeField] ParticleSystem explosionAnimation;
   
    private void OnCollisionEnter(Collision other) {

        //if (other.gameObject.tag != "Knife") {
        //    gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //}
        if (other.gameObject.tag != "Knife" && other.gameObject.tag != "Player" && gameObject.GetComponent<Rigidbody>().isKinematic != true) {
            Invoke("Explosion", timeToExplosion);
            explosionAnimation.Play();
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        Invoke("DeleteGameObject", 15);
    }

    private void Explosion() {
        Collider[] cosas = Physics.OverlapSphere(transform.position, radioExplosion);
        foreach (var cosa in cosas) {
            if (cosa.gameObject.tag == "Enemy") {
                cosa.gameObject.GetComponent<Rigidbody>().AddExplosionForce(forceExplosion, this.transform.position, radioExplosion, 120);
            }
        }

    }
    private void DeleteGameObject() {
        Destroy(this.gameObject);
    }
}
