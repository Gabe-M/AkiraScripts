using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EnemyCollision : MonoBehaviour{
    [SerializeField] private int vida;
    private Espada espada;
    [SerializeField] private int danoEspada;


    void Start(){
   
    }


    void Update(){
        
    }

    public void RecebeDano(int dano) {
        vida -= dano;

        if(vida <= 0) { Destroy(gameObject); }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Espada") {
            RecebeDano(danoEspada);
        }
        if (collision.gameObject.tag == "Bullet") {
            RecebeDano(danoEspada);
        }
    }
}
