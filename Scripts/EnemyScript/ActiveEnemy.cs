using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemy : MonoBehaviour{
   
   [SerializeField] private GameObject[] Enemys;
   bool canActive = false;
       void Start(){
        
    }

   
    void Update(){
        if(canActive)
        Active();
        
    }

      private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")){
          canActive = true;
        }
    }

    void Active(){
          for(int i = 0; i < Enemys.Length; i++){
                Enemys[i].SetActive(true);
            }
            Destroy(gameObject);
    }
}
