using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroPlayer : MonoBehaviour {
    //private enum StatusPlayer { Stop, Moving, Shooting, Jumping }
    //StatusPlayer status = StatusPlayer.Stop;
    [Header("References")]
    [SerializeField] GameObject eyesPlayer;
    [SerializeField] GameObject interactuable;
    [SerializeField] GameObject panelMissions;
    [SerializeField] Text interactuableText;
    [SerializeField] Text puntuationValueText;

    [Header("Atributtes")]
    [SerializeField] const int TOTAL_HEALTH = 100;
    [SerializeField] int health = TOTAL_HEALTH;
    [SerializeField] Text healthText;
    [SerializeField] int puntuation = 0;

    [Header("Weapons")]
    private const int TOTAL_WEAPONS = 2;
    [SerializeField] Weapon[] weapons = new Weapon[TOTAL_WEAPONS];
    private int equipedWeapon = 0;

    [Header("MenuMissions")]
    [SerializeField] GameObject[] missions;
    private int indexCurrentLevel = 1;

    private bool crounched = true;
    private string textInteractuable = "";
    private bool state;
    private bool menuMissionsAvailable;

    void Start() {
        interactuable.SetActive(false);
        panelMissions.SetActive(false);
        menuMissionsAvailable = false;
    }
    void Update() {
        healthText.text = health + " / " + TOTAL_HEALTH;
        puntuationValueText.text = puntuation + "";
        ShootAction();
        CrouchAction();

        DoAvailable_MenuMissions();
        MoveInMenuMissions();
        GoToMissionInMenuMission();

        Ray ray = new Ray(eyesPlayer.transform.position, eyesPlayer.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10)) {
            if (hit.collider.gameObject.tag == "Allie") {
                if (hit.collider.gameObject.name == "Computer_Missions") {
                    print(hit.collider.gameObject.tag + " esta siendo INTERACTUADO");
                    interactuableText.text = "Pulsa E: Abrir Misiones";
                    if (Input.GetKeyUp(KeyCode.E)) {
                        menuMissionsAvailable = true;
                        panelMissions.SetActive(true);
                    }
                }
                Debug.DrawLine(ray.origin, hit.point);
            }
        } else {
            interactuable.SetActive(false);
            menuMissionsAvailable = false;
            panelMissions.SetActive(false);

        }
    }

    private void GoToMissionInMenuMission() {
        if (Input.GetKeyUp(KeyCode.Q) && menuMissionsAvailable) {
            SceneManager.LoadScene("Level_" + indexCurrentLevel);
        }
    }
    private void MoveInMenuMissions() {
        if (Input.GetKeyUp(KeyCode.Tab) && menuMissionsAvailable) {
            if (indexCurrentLevel == missions.Length) {
                missions[indexCurrentLevel - 1].transform.Find("IconSelected_Mission").gameObject.SetActive(false);
                indexCurrentLevel = 1;
                missions[indexCurrentLevel - 1].transform.Find("IconSelected_Mission").gameObject.SetActive(true);
            } else {
                missions[indexCurrentLevel - 1].transform.Find("IconSelected_Mission").gameObject.SetActive(false);
                indexCurrentLevel++;
                missions[indexCurrentLevel - 1].transform.Find("IconSelected_Mission").gameObject.SetActive(true);
            }
        }
    }
    // We check when the Missions Menu is open or not
    private void DoAvailable_MenuMissions() {
        if (menuMissionsAvailable) {
            interactuable.SetActive(false);
        } else {
            interactuable.SetActive(true);
            indexCurrentLevel = 1;
        }
    }
    public void ShootAction() {
        if (Input.GetMouseButton(0)) {
            //weapons[equipedWeapon].GetComponent<Animator>().SetBool("Shooting", true);
            weapons[equipedWeapon].PullTrigger();
        } else {
            weapons[equipedWeapon].GetComponent<Animator>().SetBool("Shooting", false);
        }
    }
    private void JumpAction() {
        //For now, doesn't has any use
    }
    private void CrouchAction() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (crounched) {
                GetComponent<CharacterController>().height = 2f;
                crounched = false;
            } else {
                GetComponent<CharacterController>().height = 4.6f;
                crounched = true;
            }
        }
    }
    private bool IsAlive() {
        return state;
    }
    public void TakeDamage(int damage) {
        print("PLAYER recibe DAMAGES");
        health -= damage;
    }
    public void EnemyKilled(int killPoints, int healthPoints) {
        puntuation += killPoints;
        RecuperateHealth(healthPoints);

    }
    private void RecuperateHealth(int healthPoints) {
        for (int i = health; i < TOTAL_HEALTH; i++) {
            health++;
            healthPoints--;
            if (healthPoints <= 0) {
                i = TOTAL_HEALTH;
            }
        }
        
    }
}
