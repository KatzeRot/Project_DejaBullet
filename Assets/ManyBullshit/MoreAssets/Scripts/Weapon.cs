using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
    bool activate;
    private int bulletsToReload;
    [SerializeField] protected AudioSource shootSound;
    [SerializeField] protected Text bullets;
    [SerializeField] protected Text nameGunText;
    [Header("Caracteristicas")]
    [SerializeField] protected float powerShoot;
    [SerializeField] protected int bulletsMagazine;
    [SerializeField] protected int MAX_BulletsMagazine;
    [SerializeField] protected int total_Bullets;


    //protected AudioSource source;

    protected bool reloading = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public virtual void PullTrigger() {
        shootSound.Play();
    }
    public virtual void MakeDamageToEnemy() {

    }
    public void ReloadWeapon() {
        for (int i = bulletsMagazine; i < MAX_BulletsMagazine; i++) {
            bulletsMagazine++;
            total_Bullets--;
            if (total_Bullets <= 0) {
                i = MAX_BulletsMagazine;
            }
        }
        transform.Rotate(55, 0, 0);
        reloading = false; // CUANDO "reloading" SE VUELVE <<FALSE>>, PERMITE VOLVER A DISPARAR DESPUÉS DE RECARGAR
    }
    //USAMOS ESTE METODO CUANDO ESTA RECARGANDO.
    public bool Reloading() {
        reloading = true;
        transform.Rotate(-55, 0, 0);
        return reloading;
    }

    public int GetBulletsMagazine() {
        return bulletsMagazine;
    }
    public int GetMaxBulletsMagazine() {
        return MAX_BulletsMagazine;
    }
    public int GetTotalBullets() {
        return total_Bullets;
    }
    public void SetTotalBullets(int bullets) {
        total_Bullets += bullets;
    }

}
