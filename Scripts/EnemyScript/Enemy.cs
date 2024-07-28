using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {
    #region Shoot
    [SerializeField] private Transform barrel;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootRate;
    private float nextShoot;
    private float limite;
    private Collider2D range;

    #endregion
    [SerializeField] private int vida;
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float radios;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    #region Start
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start() {

    }
    /*private void OnDrawGizmos() {
       Gizmos.DrawCube(new Vector3(transform.position.x + 0.063f, transform.position.y + -0.85f)
           , new Vector2(0.05f, 0.05f));
       Gizmos.DrawCube(new Vector3(transform.position.x + -0.063f, transform.position.y + -0.85f)
           , new Vector2(0.05f, 0.05f));

   }*/
    #endregion
    void Update() {
        range = Physics2D.OverlapCircle(transform.position, radios, playerLayer);
        if (range == true) {
            Move();
        }
        if (range == true) {
            Shoot();
        }
    }
    private void Move() {
        Vector2 posicaoAlvo = player.position;                                                    // verifica a posicao do alvo
        Vector2 posicaoAtual = transform.position;                                              // verifica a propria posicao
        float distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);                          // Ve propria distancia com o player
        Vector2 direcao = posicaoAlvo - posicaoAtual;                                           // Verifica a direcao do alvo
        Vector2 direcaoX = new Vector2(direcao.normalized.x, 0);                                // normaliza a direcao a colando entre 0 e 1
        rb.velocity = (speed * direcaoX);                                                             // velocidadeMovimento = 2 e direcao = (1, -1) -> (2, -2)
        if (rb.velocity.x > 0) {
            sprite.flipX = false;
        }
        if (rb.velocity.x < 0) {
            sprite.flipX = true;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radios);
    }
    private void Shoot() {
        if (UnityEngine.Time.time > nextShoot) {
            nextShoot = UnityEngine.Time.time + shootRate;
            Instantiate(bullet, barrel.position, barrel.rotation);

            
        }

    }
    public void RecebeDano(int dano) {
        vida -= dano;

        if (vida <= 0) { Destroy(gameObject); }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Bullet") {
            RecebeDano(5);
        }
        if (collision.gameObject.tag == "Espada") {
            RecebeDano(5);
        }
    }
}

