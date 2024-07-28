using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.U2D;

public class BikerController : MonoBehaviour{
    public float speed;
    public PlayerController playerCon;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float radios;
    private Collider2D range;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private TimeLineController timeLine;



    void Start(){
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        timeLine = FindAnyObjectByType<TimeLineController>();
    }


    void Update(){
        range = Physics2D.OverlapCircle(transform.position, radios, playerLayer);
        if (range == true) {
            Move();
        }
        speed = FindAnyObjectByType<PlayerController>().speed;
        playerCon = FindAnyObjectByType<PlayerController>();
        transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z);
    }
    private void Move() {
        Vector2 posicaoAlvo = player.position;                                                
        Vector2 posicaoAtual = transform.position;                                            
        float distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);                          
        Vector2 direcao = posicaoAlvo - posicaoAtual;                                          
        Vector2 direcaoX = new Vector2(direcao.normalized.x, 0);

        anim.SetFloat("Speed", speed);

        if (distancia > 2) {
            rb.velocity = (speed * direcaoX);
            anim.SetBool("Move", true);
        } else { 
            rb.velocity = (new Vector2(0,0));
            anim.SetBool("Move", false);
        }

        if(rb.velocity.x != 0) {
            anim.SetBool("Move", true);
        } else {
            anim.SetBool("Move", false);
        }

        if (rb.velocity.x > 0) {
            sprite.flipX = false;
        }
        if (rb.velocity.x < 0) {
            sprite.flipX = true;
        }if(playerCon.isDown == true){
               anim.SetBool("isDown", true);
        }else{
               anim.SetBool("isDown", false);
        }

    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radios);
    }
}
