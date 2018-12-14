using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipGoForward : MonoBehaviour {
    [SerializeField] float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(Vector3.forward * speed *Time.deltaTime);
	}
}
