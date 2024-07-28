using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBat : MonoBehaviour{
    PlayerController PlayerClass;
    DIalogoController dIalogoController;
    [SerializeField] int dano;
    [Header("Move")]
    #region Move
    private bool FlipX;
    [SerializeField] private float distancia;
    #endregion
    [Header("MoveDeafault")]
    #region MoveDeafault
    [SerializeField] private bool isFront = false;
    [SerializeField] private float wallOffSett;
    [SerializeField] private float wallRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float speedDefault;
    [SerializeField] float WallCount = 0;
    [SerializeField] private bool FlipDefault = false;
    [SerializeField] float FlipCooldown;
    [SerializeField] float flipTime;
    [SerializeField] Collider2D rightWall;
    [SerializeField] Collider2D leftWall;
    bool back;
    float Flipx;
    #endregion
    [Header("rigidbody")]
    #region rigidbody
    [SerializeField] private int vida;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    #endregion
    [Header("FollowPlayer")]
    #region FollowPlayer
    [SerializeField] private bool isfollow = false;
    [SerializeField] private bool range;
    [SerializeField] GameObject visionsOfPlayer;
    [SerializeField] GameObject visionsOfGround;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Transform visaoObj;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField]private PolygonCollider2D polygonCollider;

    [SerializeField] private float radios;
    [SerializeField] private float distanciaMin;
    private bool isGotOut = false;
    bool jaViu = false;
    [SerializeField] private bool estaVendo = false;
    [SerializeField] private float angle;
    [SerializeField] private bool miss;
    [SerializeField] private float angleRate;
    [SerializeField] private float angleTime;
    [SerializeField] bool isHidding = false;
    [SerializeField] bool stop;
    [SerializeField] bool start;
    int localscaleX;
    #endregion
    [Header("FollowPlayer")]
    #region
    [SerializeField] float attackRate;


    Vector2 posicaoAlvo;
    Vector2 posicaoAtual;
    Vector2 visao;
    Vector2 direcao;
    Vector3 direcaoX;
    float nextAttack;
    [SerializeField] bool isListening;
    #endregion
    [Header("Itens")]
    #region itens
    [SerializeField] private GameObject KeyObj;
    #endregion



    #region Start
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

    }
    void Start() {
        speedDefault = speed;
    }
    #endregion
    void Update() {
        
        Classes();
        ProcurarPlayer();
        if (range && estaVendo && isFront || range && estaVendo && isListening) {
            jaViu = true;
             isfollow = true;
            FollowPlayer();
            Attack();
        }
        else {
            MoveDefault();
            isfollow = false;
        }
        Angle();
        PickPocket();
    }

    void MoveDefault() {
         rightWall = Physics2D.OverlapCircle(transform.position + new Vector3(wallOffSett, 0), wallRadius, groundLayer);
         leftWall = Physics2D.OverlapCircle(transform.position + new Vector3(-wallOffSett, 0), wallRadius, groundLayer);

        if (leftWall != null) {
            speedDefault = speed;
            if (UnityEngine.Time.time > flipTime) {
                flipTime = UnityEngine.Time.time + FlipCooldown;

            }
            FlipDefault = true;

        }
        if (rightWall != null) {
            speedDefault = -speed;
            if (UnityEngine.Time.time > flipTime) {
                flipTime = UnityEngine.Time.time + FlipCooldown;

            }
            FlipDefault = false;
        }
        if (!jaViu && !stop) {
            rb.velocity = new Vector2(speedDefault, 0);
        }
        if(direcao.x > 0 && speedDefault > 0) {
            isFront = true;
        } else { isFront = false; }
        if (direcao.x < 0 && speedDefault < 0) {
            isFront = true;
        } else { isFront = false; }
        #region isListening
        if (PlayerClass.run && distancia < 20 && !PlayerClass.isDown) { 
            isListening = true;
        }else if (distancia > 8 && PlayerClass.walk){
            isListening = false;
        }else if (distancia < 8 && PlayerClass.walk && !PlayerClass.isDown) {
            isListening = true;
         }else if (PlayerClass.isDown) {
            isListening = false;
         }else if(distancia > 20) {
            isListening = false;
        }
        #endregion

    }

    void ProcurarPlayer() {
        //   range = Physics2D.OverlapCircle(transform.position, radios, playerLayer);
        posicaoAlvo = player.transform.position;
        posicaoAtual = transform.position;
        visao = visaoObj.position;
        direcao = posicaoAlvo - visao;
        distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);
        direcaoX = new Vector2(direcao.normalized.x, 0);
        //Debug.Log(distancia);

        RaycastHit2D visionGround = Physics2D.Raycast(visao, direcao, distancia, groundLayer);
        RaycastHit2D visionPlayer = Physics2D.Raycast(visao, direcao, distancia, playerLayer);
        if (visionGround.transform == null) {
            if (visionPlayer.transform != null) {
                visionsOfPlayer = visionPlayer.transform.gameObject;
                if (visionPlayer.transform.tag == "Player" && range && !isHidding) {
                    estaVendo = true;
                    miss = true;
                }
                else {
                    estaVendo = false;
                }
            }
            else {
                estaVendo = false;

            }
        }
        else {
            visionsOfGround = visionGround.transform.gameObject;
            estaVendo = false;

        }
        if(estaVendo && range) {
            isGotOut = false;
            //stop = false;
        }
        if (!estaVendo && jaViu || !range && jaViu) {
            if (miss) {
                angleTime = UnityEngine.Time.time + angleRate;
                isGotOut = true;
                miss = false;

            }
            if (isGotOut) {
                stop = true;
                if (UnityEngine.Time.time > angleTime || estaVendo && range) {
                    isGotOut = false;
                    stop = false;
                }
            }
            else {
       
                rb.velocity = new Vector2(speedDefault, 0);
            }
        }
    
        if (rb.velocity.x > 0) {
            localscaleX = 1;
        }
        if(rb.velocity.x < 0) {
            localscaleX = -1;
        }
        transform.localScale = new Vector3(localscaleX, 1, 1);
      
        if (stop) {
            rb.velocity = Vector2.zero;
        }
        if (rb.velocity.x != 0) {
            anim.SetBool("Run", true);
        }else if (rb.velocity.x == 0) {
            anim.SetBool("Run", false);

        }
        if (isHidding && dIalogoController.DialogoCount > 6) {
            start = true;
        }
    }
        

    private void FollowPlayer() {
      
        if (start) {
            rb.velocity = direcaoX * (speed + 3);
            stop = false;
        }
           
    }

    private void Angle(){
        if (estaVendo && range) {
            //Rotacao
            Vector2 posicaoAlvo = player.position;
        Vector2 posicaoAtual = transform.position;
        float distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);
        Vector2 direcao = posicaoAlvo - posicaoAtual;
        var dir = direcao;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    
        }

    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radios);
      //  Gizmos.DrawLine(visaoObj.position, player.position);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(wallOffSett, 0), wallRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-wallOffSett, 0), wallRadius);
    }

    void Attack() {
        if (distancia < 2 && distancia > -2 && UnityEngine.Time.time > nextAttack) {
            nextAttack = UnityEngine.Time.time + attackRate;
            PlayerClass.RecebeDano(dano);
            anim.SetTrigger("Attack");
        }
      
    }
         
        
    

    public void RecebeDano(int Dano) {
        vida -= Dano;

        if (vida <= 0) {
             Destroy(gameObject);
              Instantiate(KeyObj, transform.position, Quaternion.identity);
        }

    }

  

   

    void PickPocket() {
        if (Input.GetKeyDown(KeyCode.Q) && distancia > 2 && !isFront && !isfollow) {
            Instantiate(KeyObj, transform.position, Quaternion.identity);
        }
    }

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            RecebeDano(5);

        }
        if (collision.gameObject.tag == "Espada") {
            RecebeDano(5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            // range = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            range = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            range = false;
        }
    }
    #endregion

    void Classes() {
        PlayerClass = FindAnyObjectByType<PlayerController>();
        isHidding = FindAnyObjectByType<PlayerController>().isHidding;
        dIalogoController = FindAnyObjectByType<DIalogoController>();
    }
}

