using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punkGun : MonoBehaviour{
 float nextShoot;
 [SerializeField] float shootRate;
 [SerializeField] GameObject bullet;
 [SerializeField] Transform barrel;
 [SerializeField] Transform player;
 [SerializeField] Animator anim;
  float angle;
 bool isFront;
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        Shoot();
    }
      private void Shoot() {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
        if (UnityEngine.Time.time > nextShoot) {
            nextShoot = UnityEngine.Time.time + shootRate;
            Instantiate(bullet, barrel.position, barrel.rotation);
           
            anim.SetTrigger("Shoot");
        } 
      }
       

   }
    
}
