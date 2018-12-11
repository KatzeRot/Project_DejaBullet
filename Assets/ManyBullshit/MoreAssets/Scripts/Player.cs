using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Vector3 positionWeapon;
  
    [Header("ESTADO")]
    [SerializeField] bool alive = true;
    [SerializeField] int health = 20;

    [Header("ACCIONES")]
    [SerializeField] int speed;
    [SerializeField] int jump;
    [SerializeField] int damage = 2;

    [Header("EQUIPO")]
    private const int NUMWEAPON = 2;
    [SerializeField] Weapon[] weapons = new Weapon[NUMWEAPON];
    private int equipedWeapon;

    [Header("REFERENCIAS")]
    [SerializeField] Text healthPoints;
    [SerializeField] Text contBullets;
    [SerializeField] Text shootgunBullets;
    GameObject player;
    bool keyTaked = false;
    [SerializeField] GameObject doorFortress;
    [SerializeField] GameObject bossDoor;
    [SerializeField] GameObject wall;
    
    public bool IsAlive() {
        return alive;
    }
    public void Shoot() {
        weapons[equipedWeapon].PullTrigger();
    }
    public void Healing(int heal) {
        health = health + heal;
    }
    public void Run() {

    }
    public void Jump() {

    }
    public void SelectWeapon(int indexWeapon) {
        foreach(Weapon weapon in weapons) {
            weapons[indexWeapon].gameObject.SetActive(true);
        }
    }
    public void QuitWeaponMove(int indexWeapon) {
        foreach (Weapon weapon in weapons) {
            //weapons[indexWeapon].transform.Translate(Vector3.down * 5);
        }
    }
    public void QuitWeapon(int indexWeapon) {
        foreach (Weapon weapon in weapons) {
            QuitWeaponMove(indexWeapon);
            weapons[indexWeapon].gameObject.SetActive(false);
        }
    }
    public void Start() {
        QuitWeapon(1);
        positionWeapon = weapons[0].transform.position;
    }
    public void Update() {
        healthPoints.text = health.ToString();
        if(health < 6) {
            healthPoints.color = Color.red;
        } else {
            healthPoints.color = Color.white;
        }
        if (Input.GetMouseButton(0)) {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            QuitWeapon(1);
            SelectWeapon(0);
            equipedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            QuitWeapon(0);
            SelectWeapon(1);
            equipedWeapon = 1;
        }

    }
    //EN DESUSO POR AHORA
    //public void Scope(int weaponIndex) {
    //    Transform weapon = weapons[weaponIndex].transform;
    //    //if (weapon.position.x >= 0 && weapon.position.y >= -0.98) {
    //    //    weapon.Translate(-0.01f, 0.01f, 0);
    //    //}
    //    weapon.transform.position = new Vector3(0, 0.98f, 0);
        
    //}
    ////EN DESUSO POR AHORA
    //public void NormalStateGun(int weaponIndex) {
    //    Transform weapon = weapons[weaponIndex].transform;
    //    if (weaponIndex == 0) {
    //        weapon.SetPositionAndRotation(new Vector3(1.06f, -1.61f, 2.176f), weapon.rotation);
    //    } else if (weaponIndex == 1) {
    //        weapon.SetPositionAndRotation(new Vector3(0.6f, -0.5f, 1.675f), weapon.rotation);
    //    }
        
    //}

    public void TakeDamage(int damage) {
        health = health - damage;
        if (health <= 0) {
            health = 0;
            Die();
        }
    }
    public void Die() {
        alive = false;
    }
    public void TakeBulletsToShotgun(int bullets) {
        weapons[1].GetComponent<Weapon>().SetTotalBullets(bullets);
    }

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Key") {
            print("Llave cogida");
            keyTaked = true;
        }
        if (other.gameObject.tag == "Zone" && keyTaked) {
            print("Puertas ABIERTAS");
            doorFortress.transform.Translate(0, -50, 0);
            bossDoor.transform.Translate(0, -60, 0);
            wall.transform.Translate(15,0, 0);
            keyTaked = false;
        }

    }
}
