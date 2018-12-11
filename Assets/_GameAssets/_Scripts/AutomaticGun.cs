using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticGun : Weapon {
    private Animator gunAnimator;
    [SerializeField] string nameGun;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject generatorPoint;
    [SerializeField] GameObject FPSController;
    [SerializeField] GameObject bulletPrefab;
    private AudioSource audioGunShoot;
    private AudioSource audioNoAmmo;
    [Header("Caracteristicas")]
    [SerializeField] float distanceAttack = 100; //Default value
    [SerializeField] protected float cadence;
    
    
    private float otherShoot = 0;
    // Use this for initialization
    void Start() {
        gunAnimator = GetComponent<Animator>();
        otherShoot = cadence;
        audioGunShoot = GetComponentInChildren<AudioSource>();
        audioNoAmmo = audioGunShoot.gameObject.GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        bullets.text = bulletsMagazine + " / " + total_Bullets;
        nameGunText.text = nameGun;
        otherShoot += 0.1f;
        
        // Check if I have bullets to reload and then if I pr
        if (total_Bullets > 0 && Input.GetKeyDown("r") && bulletsMagazine < MAX_BulletsMagazine && !reloading) {
            Reloading();
            Invoke("ReloadWeapon", 1);
        }
        //if (bulletsMagazine <= 0) {
        //    contBullets.color = Color.red;
        //} else {
        //    contBullets.color = Color.white;
        //}
    }
    public override void PullTrigger() {
        print("DISPARA!");
        if (bulletsMagazine > 0 && !reloading && otherShoot >= cadence) {
            gunAnimator.SetBool("Shooting", true);
            audioGunShoot.Play();
            //audioNoAmmo.Play();
            bulletsMagazine--;
            float x = Random.Range(0, 1.5f);
            float y = Random.Range(0, 1.5f);
            mainCamera.transform.Rotate(0.5f, 0.5f, 0); //Shake de camera
            generatorPoint.transform.Rotate(x, y, 0);
            otherShoot = 0;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = new Ray(generatorPoint.transform.position, generatorPoint.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceAttack)) {
                if (hit.collider.gameObject.tag == "Enemy") {
                    print(hit.collider.gameObject.tag + " tocado");
                    hit.collider.gameObject.GetComponent<EnemyGuard>().TakeDamage(powerShoot); //Realiza el daño al enemigo impactado
                    Debug.DrawRay(generatorPoint.transform.position, generatorPoint.transform.forward * distanceAttack, Color.red, 1);
                }
                Debug.DrawLine(ray.origin, hit.point);
            }
            Debug.DrawRay(generatorPoint.transform.position, generatorPoint.transform.forward * distanceAttack, Color.blue, 1);
            generatorPoint.transform.rotation = new Quaternion(0f,0f,0f,0f); //Default rotation of the generator bullet
        }else if(bulletsMagazine == 0 || otherShoot < cadence){
            gunAnimator.SetBool("Shooting", false);
        }
    }
}

