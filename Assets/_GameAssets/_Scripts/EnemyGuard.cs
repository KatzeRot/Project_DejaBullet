using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyGuard : MonoBehaviour {
    private enum Status { Shooting, Idle, Waiting, Running, Damaged }
    Status status = Status.Idle;

    private NavMeshAgent agent;
    private Animator animatorEnemyGuard;
    RaycastHit rch;

    [Header("StatusCharacter&References")]
    [SerializeField] bool state = true; //Check if is alive or death
    [SerializeField] Transform positionCombat; //The position where he has to be when the PLAYER has been founded
    [SerializeField] GameObject eyesWatcher;
    [SerializeField] GameObject shootPoint;
    [SerializeField] GameObject player;
    private bool damaged = false;
    [SerializeField] bool followPlayer;
    [SerializeField] float health = 20;
    [SerializeField] float distanceValue = 25.0f;
    [SerializeField] float angleValue = 62f;
    [SerializeField] int damage = 1;
    [SerializeField] float cadence;
    [SerializeField] int killPoints;
    [SerializeField] int healthPoints;
    private float otherShoot = 0;


    void Start() {
        otherShoot = cadence;
        agent = GetComponent<NavMeshAgent>();
        animatorEnemyGuard = GetComponent<Animator>();
        killPoints = 100;
        healthPoints = 10;
    }
    void Update() {
        if (IsAlive()) {
            otherShoot += 0.1f;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 direccion = Vector3.Normalize(player.transform.position - transform.position);
            float angulo = Vector3.Angle(direccion, transform.forward);
            Vector3 playerPosition = new Vector3(player.transform.position.x,
                                            this.transform.position.y,
                                            player.transform.position.z);
            if(damaged == true && status == Status.Idle) {
                
            }else if(damaged == true && status == Status.Waiting){
                status = Status.Shooting;
            }
            switch (status) {
                case Status.Idle: //When he doesn't see the PLAYER
                    Ray ray = CreateRayCast(eyesWatcher.transform.position, eyesWatcher.transform.forward); ;
                    if (distance < distanceValue && angulo < angleValue) {
                        if (Physics.Raycast(ray, out rch, Mathf.Infinity) && rch.transform.gameObject.name == "HeroController") {
                            print("Identificado: " + rch.collider.gameObject.name);
                            status = Status.Running; //When the PLAYER has been detected, he pass to running status
                        }
                    }else if(damaged == true && status == Status.Idle) { 
                        status = Status.Running;
                    }
                    break;
                case Status.Shooting: //When he has seen the PLAYER and has visual contact with him
                    print("EMPEZANDO A DISPARAR");
                    //agent.isStopped = true;
                    this.transform.LookAt(playerPosition);
                    Ray rayShoot = CreateRayCast(shootPoint.transform.position, shootPoint.transform.forward);
                    eyesWatcher.transform.LookAt(player.transform.position);
                    shootPoint.transform.LookAt(player.transform.position);
                    print("DEBERÍA DISPARAR Y ENCONTRAR AL PLAYER");
                    if (Physics.Raycast(rayShoot, out rch, Mathf.Infinity) && rch.transform.gameObject.name != "HeroController") {
                        print("Se ha perdido el CONTACTO con el PLAYER");
                        //this.transform.LookAt(playerPosition);
                        status = Status.Waiting; //When he arrived to his position, he pass to waiting status
                    } else if (otherShoot >= cadence) {
                        ShootToPlayer(rayShoot);
                        animatorEnemyGuard.SetBool("Shooting", true);
                    }
                    break;
                case Status.Waiting: //When he has seen the PLAYER but hasn't visual contact
                if(followPlayer){
                    status = Status.Running;
                }else{
                    shootPoint.transform.rotation = new Quaternion(0f, 0f, 0f, 0f); //Default rotation of the generator bullet
                    animatorEnemyGuard.SetBool("EnemyFounded", true);
                    animatorEnemyGuard.SetBool("Shooting", false);
                    ray = CreateRayCast(eyesWatcher.transform.position, eyesWatcher.transform.forward);
                    if (Physics.Raycast(ray, out rch, Mathf.Infinity) && rch.transform.gameObject.name == "HeroController") {
                        print("Identificado: " + rch.collider.gameObject.name);
                        this.transform.LookAt(playerPosition);
                        status = Status.Shooting; //When he arrived to his position, he pass to waiting status
                    }
                }
                    
                    break;
                case Status.Running: //When he isn't in his position and is moving to there
                    if(followPlayer){
                        agent.isStopped = false;
                        agent.destination = player.transform.position;
                        ray = CreateRayCast(eyesWatcher.transform.position, eyesWatcher.transform.forward);
                        if (Physics.Raycast(ray, out rch, Mathf.Infinity) && rch.transform.gameObject.name == "HeroController") {
                            print("Identificado: " + rch.collider.gameObject.name);
                            status = Status.Shooting; //When he arrived to his position, he pass to waiting status
                        }
                    }else if(positionCombat != null) {
                        animatorEnemyGuard.SetBool("Running", true);
                        agent.destination = positionCombat.position;
                    }else{
                    
                        status = Status.Waiting; //When he arrived to his position, he pass to waiting status
                    }
                    break;
                case Status.Damaged: //When he isn't in his position and is moving to there
                    // Animation

                break;
            }
        }

    }
    private void ShootToPlayer(Ray ray) {
        //Ray rayShootEnemy = new Ray(eyesWatcher.transform.position, eyesWatcher.transform.forward);
        float x = Random.Range(0, 5f);
        float y = Random.Range(0, 5f);
        shootPoint.transform.Rotate(x, y, 0);
        animatorEnemyGuard.SetBool("Shooting", true);
        otherShoot = 0;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            if (hit.collider.gameObject.tag == "Player") {
                print(hit.collider.gameObject.tag + " tocado");
                hit.collider.gameObject.GetComponent<HeroPlayer>().TakeDamage(damage); //Realiza el daño al PLAYER
                Debug.DrawLine(shootPoint.transform.position, shootPoint.transform.forward * Mathf.Infinity, Color.yellow, 1);
            }
            Debug.DrawLine(ray.origin, shootPoint.transform.forward * Mathf.Infinity, Color.yellow, 1);
        }
        Debug.DrawRay(shootPoint.transform.position, shootPoint.transform.forward * Mathf.Infinity, Color.magenta, 2);
    }

    private Ray CreateRayCast(Vector3 origin, Vector3 destination) {
        eyesWatcher.transform.LookAt(player.transform.position);
        Ray ray = new Ray(origin, destination);
        return ray;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "positionCombat") {
            print("Tocado el TRIGGER");
            animatorEnemyGuard.SetBool("Running", false);
            agent.isStopped = true;
            status = Status.Waiting; //When he arrived to his position, he pass to waiting status
        }
    }
    private bool IsAlive() {
        if(this.health <= 0){
            state = false;
            animatorEnemyGuard.SetBool("Death", true);
            player.GetComponent<HeroPlayer>().EnemyKilled(killPoints, healthPoints);
            Destroy(this);
            //animatorEnemyGuard.SetBool("Death", false);
        }
        return state;
    }
    public void TakeDamage(float damage) {
        //watcherAnimator.SetTrigger("Hit");
        damaged = true;
        health -= damage;
    }
}