using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Transform player;
    [SerializeField] private float offset;
    [SerializeField] private GameObject espada;
    [SerializeField] private bool isFront = false;
    [SerializeField] int angleModA = -60;
    [SerializeField] int angleModB = -100;
    private SpriteRenderer spriteRender;
    bool isAngle = false;
    float angle = 0;
    float localscaleZ;
    float localscaleX = 1;
    bool canAttack = false;
    bool isDistance = false;
    [SerializeField] bool armMove = false;

    private void Start() {
        spriteRender = GetComponent<SpriteRenderer>();
    }
    void Update() {
        HandleAiming();
        if(isDistance)
        Posicao();
        
    }

    void HandleAiming() {

            //Rotacao
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
           angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        #region isFront
        if (player.localScale.x > 0) {
            isFront = true;
        }
        else {
            isFront = false;
        }
        if (isFront && angle > -70 && angle < 90 || !isFront && angle > -110 && angle <= 90) {
            armMove = true;
        }else {
            armMove = false;
        }
        #endregion
        if (isFront && armMove || !isFront && !armMove) {
            // Posicao
            Vector3 playerToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
            playerToMouseDir.z = 0;
            if(playerToMouseDir.x > 1.2f || playerToMouseDir.x < -1.2f){
                isDistance = true;
            transform.position = player.position + (offset * playerToMouseDir.normalized);
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + 1);
               transform.eulerAngles = new Vector3(0, 0, angle); 
            }else{
                if(playerToMouseDir.y < 1.5f && playerToMouseDir.y > -0.5f){
                isDistance = false;
                if(isFront){
                 transform.position = new Vector3(player.position.x + 0.37f, player.position.y - 0.15f, player.position.z + 1);
                 transform.eulerAngles = new Vector3(0, 0, -70); 
            }else{
                transform.position = new Vector3(player.position.x - 0.375f, player.position.y - 0.15f, player.position.z + 1);
                 transform.eulerAngles = new Vector3(0, 0, -120);
            }
                }else{
                          isDistance = true;
            transform.position = player.position + (offset * playerToMouseDir.normalized);
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + 1);
               transform.eulerAngles = new Vector3(0, 0, angle); 
                }
            }

            //Girar
            Vector3 localScale = Vector3.one;

            if (angle > 90 || angle < -90) {
                localScale.y = -1f;
                localScale.x = -1f;
            }
            else {
                localScale.y = 1f;
                localScale.x = 1f;

            }

         //    print(playerToMouseDir.y);
            transform.localScale = localScale;
        }
    }
    void Espada() {
     Instantiate(espada, transform.position, transform.rotation);
        
    }
    void Posicao() {
        #region isFront
        if (player.localScale.x > 0) {
            isFront = true;
        }
        else {
            isFront = false;
        }
        #endregion
        if(isFront) {
            if (angle >= -70 && angle < -60) {
                transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.3f, transform.position.z);
               // print("1");
            }
            else if (angle >= -60 && angle < -50) {
                transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.3f, transform.position.z);             //   print("2");
            }
            else if (angle >= -50 && angle < -30) {
                transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.3f, transform.position.z);              //  print("3");
            }
            else if (angle >= -30 && angle < -20) {
                transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.2f, transform.position.z);               // print("4");
            }
            else if (angle >= -20 && angle < -15) {   
                transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.1f,  transform.position.z);
              //  print("5");
            }
            else if (angle >= -15 && angle < 30) {
                transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y + 0.11f, transform.position.z);
               // print("6");
            }
            else if (angle >= 30 && angle < 60) {
                transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y + 0.13f, transform.position.z);
              //  print("7");
            }
            else if (angle >= 60 && angle < 90) {
                transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 0.15f, transform.position.z);
             //   print("8");
            }

        }
        else if (!isFront) {
            if (angle >= 90 && angle < 125){
                transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y + 0.15f, transform.position.z);               // print("a");
            }
            else if (angle >= 125 && angle < 155){ 
                transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y + 0.13f, transform.position.z);
                print("b");
            }   
           else if (angle >= 155 && angle < 180) { 
                transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.11f, transform.position.z);
                print("c");
            }
            else if (angle >= -179 && angle < -170) {  
                transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.1f, transform.position.z);
                print("d");
            }
            else if (angle >= -170 && angle <  - 160) { 
                transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.2f, transform.position.z);
                print("e");
            }
            else if (angle >= -160 && angle < -140) { 
                transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.3f, transform.position.z);
                print("f");
            }
            else if (angle >= -140 && angle < -130) { 
                transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.3f, transform.position.z);
                print("g");
            }
            else if (angle >= -130 && angle < -110) { 
                transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.3f, transform.position.z);
                print("h");
            }
            else {
                if (!armMove) {
                    transform.position = new Vector3(transform.position.x - 0.15f, transform.position.y + 0.11f, transform.position.z);
                    print("c");
                }

            }
      
        }
       
    }
}
