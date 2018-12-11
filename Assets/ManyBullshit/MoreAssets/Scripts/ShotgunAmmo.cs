using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAmmo : MonoBehaviour {

    int rotationVector;
    int bullets = 4;
    [SerializeField] AudioClip sound;
    // Update is called once per frame
    void Update() {
        ++rotationVector;
        transform.rotation = Quaternion.Euler(0f, rotationVector, 0f);
    }
    private void OnTriggerEnter(Collider other) {
        GameObject n = other.gameObject;
        if (other.gameObject.tag == "Player") {
            GetComponent<AudioSource>().PlayOneShot(sound);
            n.GetComponent<Player>().TakeBulletsToShotgun(bullets);
            Destroy(this.gameObject);
        }
    }
}