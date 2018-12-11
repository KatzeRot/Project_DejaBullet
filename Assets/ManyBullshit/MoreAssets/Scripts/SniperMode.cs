using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMode : MonoBehaviour {
    [SerializeField] Transform shootPoint;
    [SerializeField] Camera miCamara;
    bool apuntando = false;
    float currentZoom;
    float minZoomLevel = 25;
    float maxZoomLevel;
    private void Start() {
        maxZoomLevel = miCamara.fieldOfView;
        currentZoom = miCamara.fieldOfView;
    }
    void Update () {
        if (Input.GetMouseButtonDown(1)) {
            apuntando = true;
        }
        if (Input.GetMouseButtonUp(1)) {
            apuntando = false;
        }
        if (apuntando) {
            currentZoom -= (Time.deltaTime);
        }
    }
    private void OnMouseDown() {
        Vector3 forward = shootPoint.forward;
        //Rayo de disparo
        Ray rayo = new Ray(shootPoint.position, forward);
        //Declaramos el objeto que recoge el impacto
        RaycastHit hitInfo;
        //Lanzamos el rayo
        bool target = Physics.Raycast(rayo, out hitInfo);
        if (target) {
            print(hitInfo.collider.gameObject.name);
        } else {
            print("Bullshit");
        }
    }
}
