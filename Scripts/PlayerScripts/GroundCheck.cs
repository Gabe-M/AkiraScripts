using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GroundCheck : MonoBehaviour{

    public GameObject playerController;
    PlayerController a;
    bool isJumpingG;
    void Start(){
       
    }

    void Update(){
        Instance();
    }

    private void Instance() {
        isJumpingG = playerController.GetComponent<PlayerController>().isJumping;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Ground") {
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        
    }
}
