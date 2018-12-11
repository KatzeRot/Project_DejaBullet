using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeLauncher : MonoBehaviour {

    [SerializeField] Transform genPoint;
    [SerializeField] GameObject prefabKnife;
    [SerializeField] int force = 1800;

    void Update() {
        if (Input.GetKeyDown("f")) {
            GameObject proyectil = Instantiate(prefabKnife, genPoint.transform.position, genPoint.transform.rotation);
            proyectil.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * force);
            proyectil.GetComponent<Rigidbody>().AddTorque(Vector3.forward * 1000);
        }
    }
}