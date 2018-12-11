using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPlayer : MonoBehaviour {
    private enum StatusPlayer { Stop, Moving, Shooting, Jumping }
    StatusPlayer status = StatusPlayer.Stop;

    [SerializeField] const int TOTAL_HEALTH = 100;
    [SerializeField] int health = TOTAL_HEALTH;
    [SerializeField] Text healthText;
    private const int TOTAL_WEAPONS = 2;
    [SerializeField] Weapon[] weapons = new Weapon[TOTAL_WEAPONS];
    private int equipedWeapon = 0;

    private bool crounched = true;
    private bool state;

    void Start() {

    }
    void Update() {
        healthText.text = health + " / " + TOTAL_HEALTH;
        ShootAction();
        CrouchAction();
    }

    // Update is called once per frame
    void FixedUpdate() {

        switch (status) {
            case StatusPlayer.Stop:
                break;
            case StatusPlayer.Shooting:
                ShootAction();
                break;
                //case StatusPlayer.Jumping:
                //    JumpAction();
                //    break;
        }
    }
    public void ShootAction() {
        if (Input.GetMouseButton(0)) {
            //weapons[equipedWeapon].GetComponent<Animator>().SetBool("Shooting", true);
            weapons[equipedWeapon].PullTrigger();
        }else{
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
}
