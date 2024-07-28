using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GunController : MonoBehaviour {

    [SerializeField] private Transform barrel;          // Posição de onde vamos atirar
    [SerializeField] private float fireRate;            // Cadencia de tiro
    [SerializeField] private GameObject bullet;         // Projetil 
    [SerializeField] private bool isFront = false;
    [SerializeField] private Transform player;
    private float fireTimer;                            // Controle de cadencia
    float angle;

    void Start() {

    }


    void Update() {
        HandleShooting();
        Estabilize();
    }
    private void HandleShooting() {

        if (Input.GetMouseButton(0) && Canshoot()) {
            Shoot();
        }
    }

    private void Shoot() {
        fireTimer = UnityEngine.Time.time + fireRate;

        Instantiate(bullet, barrel.position, barrel.rotation);
    }

    private bool Canshoot() {
        return UnityEngine.Time.time > fireTimer;
    }
     
    private void Estabilize() {
        if (player.localScale.x > 0) {
            isFront = true;
        }
        else {
            isFront = false;
        }
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (isFront) {
            if (angle >= -70 && angle < 30) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                transform.eulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + 50);
            }else if(angle >= 30 && angle < 90) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                transform.eulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + 90);
            }
        }
    }
}

