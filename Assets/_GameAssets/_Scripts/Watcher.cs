using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Watcher : MonoBehaviour {
    public Transform[] patrolPoints = new Transform[3];
    NavMeshAgent agent;
    public GameObject player;
    float distanceValue = 50.0f;
    float angleValue = 60f;
    private Animator watcherAnimator;
    [SerializeField] GameObject eyesWatcher;

    [Header("Caracteristicas")]
    [SerializeField] float health = 20;

    //public Text textDTP;
    //public Text textATP;
    //public Text textDetected;

    enum Status { Idle, Walking, Running, Jumping, Shooting, Following, Distracted };
    Status status = Status.Idle;

    const int WAIT_TIME = 4; //TIEMPO DE ESPERA ENTRE ASIGNACIONES DE PUNTOS DE PATRULLA

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        watcherAnimator = GetComponent<Animator>();
        //AsignPatrolPoint();
    }

    void Update() {
        if (IsAlive()) {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 direccion = Vector3.Normalize(player.transform.position - transform.position);
            float angulo = Vector3.Angle(direccion, transform.forward);

            //Esto sirve para que cuando el watcher observe al player, no rote la coordenada Y
            Vector3 playerPosition = new Vector3(player.transform.position.x,
                                            this.transform.position.y,
                                            player.transform.position.z);
            ///////////////////////////////////////////////////////////////////////////////////

            eyesWatcher.transform.LookAt(player.transform.position);
            Debug.DrawLine(eyesWatcher.transform.position, player.transform.position, Color.red, 1);
            Debug.DrawRay(eyesWatcher.transform.position, player.transform.position, Color.green, 1);
            Ray ray = new Ray(eyesWatcher.transform.position, eyesWatcher.transform.forward);
            RaycastHit rch;
            if (Physics.Raycast(ray, out rch, Mathf.Infinity) && rch.transform.gameObject.name == "HeroController") {
                print("Identificado: " + rch.collider.gameObject.name);
                this.transform.LookAt(playerPosition);
                agent.destination = player.transform.position;
            }
        } else {
            watcherAnimator.SetBool("Die", true);
            Destroy(this);
        }
        //textDTP.text = "DTP: " + distance;
        //textATP.text = "ATP: " + angulo;
        //if (distance < distanceValue && angulo < angleValue) {
        //    // Lanzar RayCast para determinar si está realmente en su campo de visión
        //    Debug.DrawLine(transform.position + new Vector3(0, 5f, 0), player.transform.position, Color.red, 1);
        //    Ray ray = new Ray(eyesWatcher.transform.position, player.transform.position);
        //    RaycastHit rch;
        //    if(Physics.Raycast(ray, out rch, Mathf.Infinity)) {
        //        print("Identificado: " + rch.collider.gameObject.name);
        //        if (rch.transform.gameObject.tag == "Player") {
        //            //textDetected.color = Color.red;
        //            //textDetected.text = "ALARM!";
        //            status = Status.Following;
        //        }
        //    }
        //    // Comprobar si está distraido por un petardo
        //    //Si no esta distraido persigue a Peter y lo revienta
        //} else {
        //    //textDetected.color = Color.green;
        //    //textDetected.text = "No troubles!";
        //}
        //switch (status) {
        //    case Status.Idle:
        //        // NO HAGO NADA

        //        break;
        //    case Status.Walking:
        //        CheckDestination();
        //        break;
        //    case Status.Running:
        //        break;
        //    case Status.Jumping:
        //        break;
        //    case Status.Following:
        //        print("PLAYER encontrado. Hora de matar!!");

        //        if(distance >= 20) {
        //            watcherAnimator.SetBool("Running", true);
        //            agent.destination = player.transform.position;
        //        } else {
        //            watcherAnimator.SetBool("Running", false);
        //            //agent.isStopped = true;
        //        }
        //        break;
        //    case Status.Distracted:
        //        break;
        //}
    }

    internal void SetStatusWarning(Vector3 position) {
        if (status != Status.Following) {
            agent.destination = position;
            status = Status.Walking;
        }
    }
    int pp;
    private void AsignPatrolPoint() {
        agent.destination = patrolPoints[pp].position;
        status = Status.Walking;
        pp++;
        if (pp == patrolPoints.Length) pp = 0;
    }
    private void CheckDestination() {
        if (!agent.pathPending) {
            //print(agent.remainingDistance + ":" + agent.stoppingDistance);
            if (agent.remainingDistance <= agent.stoppingDistance + 0.1) {
                //peterAnimator.SetBool("Walking", false);
                status = Status.Idle;
                Invoke("AsignPatrolPoint", WAIT_TIME);
            }
        }
    }
    public void TakeDamage(float damage) {
        watcherAnimator.SetTrigger("Hit");
        health -= damage;
    }
    private bool IsAlive() {
        bool state = true;
        if (health <= 0) {
            state = false;
        }
        return state;
    }
}
