using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drone3 : MonoBehaviour{
    [Header("Shoot")]
    #region Shoot
    [SerializeField] private Transform barrel; //Posição de onde saiu os tiros
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootRate; //variavel auxiliar do coolDown dos tiros
    private float nextShoot; //coolDown dos tiros
    [SerializeField] private bool range; //Distancia que o Drone detecta o player
    private bool FlipX;


    #endregion
    [Header("Move")]
    #region Move
    [SerializeField] private float distancia; //Distancia do drone para o player
    #endregion
    [Header("MoveDeafault")]
    #region MoveDeafault
    [SerializeField] private float wallOffSett; 
    [SerializeField] private float wallRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float speedDefault; //Velocidade que o Drone se move normalmente
    [SerializeField] float WallCount = 0; //Quantidade de paredes que o Drone colidiu
    [SerializeField] private bool FlipDefault = false;
    [SerializeField] float FlipCooldown;
    [SerializeField] float flipTime;
    [SerializeField] Collider2D rightWall;
    [SerializeField] Collider2D leftWall;
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
    [SerializeField] GameObject visionsOfPlayer; //Verifica se o player está no campo de visão
    [SerializeField] GameObject visionsOfGround; //Verifica se uma parede está entre o player e o Drone
    [SerializeField] private Transform player; //Posição do player
    [SerializeField] private GameObject playerObj; //Objeto que constitui no player
    [SerializeField] private Transform visaoObj; //O objeto que o Drone está vendo
    [SerializeField] private LayerMask playerLayer; //Camada do player

    [SerializeField] private float radios; //Tamanho da colisão
    [SerializeField] private float distanciaMin; //Distancia mínima do drone com o player
    private bool isGotOut = false; //O player saiu da visão do drone
    bool wasSeen = false; //Verifica se o Drone ja viu o player
    [SerializeField] private bool isSeeing = false; //Verifica se o Drone está vendo o palyer
    [SerializeField] private float angle; //Angulo do player em relação ao Drone
    [SerializeField] private bool stop; //Impede que o drone se mova
    [SerializeField] private float angleRate; //Variavel auxiliar do angleTime
    [SerializeField] private float angleTime; //Tempo que o drone demora pra voltar ao angulo padrão
    [SerializeField] bool isHidding = false; //Verifica se o player está escondido

    #endregion




    #region Start
    private void Awake() { //Função que roda antes de iniciar o jogo
    //Transformando variaveis em componentes
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
        if (range == true && isSeeing) {
            wasSeen = true;
            FollowPlayer();
        }
        else {
            MoveDefault();
        }
        Angle();
    }

    void MoveDefault() { //Movimento feito quando não está perseguindo algo
        //Colisões que detectam paredes ao redor
         rightWall = Physics2D.OverlapCircle(transform.position + new Vector3(wallOffSett, 0), wallRadius, groundLayer);
         leftWall = Physics2D.OverlapCircle(transform.position + new Vector3(-wallOffSett, 0), wallRadius, groundLayer);

        if (leftWall != null) { //Colisão da parede esquerda detectou nada
            speedDefault = speed; //A velocidade padrão se torna igual ao speed
            if (UnityEngine.Time.time > flipTime) { //Timer para dar um flip
                flipTime = UnityEngine.Time.time + FlipCooldown; //Tempo que o flip será realizado

            }
            FlipDefault = true; //Define que ja foi feito um flip

        }
        if (rightWall != null) {
            speedDefault = -speed;
            if (UnityEngine.Time.time > flipTime) {
                flipTime = UnityEngine.Time.time + FlipCooldown;

            }
            FlipDefault = false; //Permite dar outro flip
        }
        if (wasSeen) {
            transform.localScale = new Vector3(Flipx, 1, 1); //Faz o drone voltar a olhar pro lado de antes de ter visto o player
        }
        if(rb.velocity.x >= 0) {
            transform.localScale = new Vector3(1, 1, 1); //Se estiver se movendo para a direita, o eixo X será positivo
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);//Se não estiver se movendo para a direita, o eixo X será negativo
        }
        if(!wasSeen)
        rb.velocity = new Vector2(speedDefault, 0); //Se ainda não viu o player então continua na velocidade padrão
    }

    void ProcurarPlayer() { //Função que detecta o player
        //   range = Physics2D.OverlapCircle(transform.position, radios, playerLayer);
        Vector2 posicaoAlvo = player.transform.position; 
        Vector2 posicaoAtual = transform.position; 
        Vector2 visao = visaoObj.position; //Ponto de inicio da visão
        Vector2 direcao = posicaoAlvo - visao; //Direção que o player está do Drone
        distancia = Vector2.Distance(posicaoAtual, posicaoAlvo); //Distancia do Drone ao player

  
        RaycastHit2D visionGround = Physics2D.Raycast(visao, direcao, distancia, groundLayer); //Colisão que detecta paredes
        RaycastHit2D visionPlayer = Physics2D.Raycast(visao, direcao, distancia, playerLayer); //Colisão que detecta o player
        if (visionGround.transform == null) { //A linha de colisão detectou nenhuma parede
            if (visionPlayer.transform != null) { //A linha de colisão detectou o player
                visionsOfPlayer = visionPlayer.transform.gameObject; //Variavel pra confirmar se o que o drone detectou é o player
                if (visionPlayer.transform.tag == "Player" && range && !isHidding) { 
                    isSeeing = true;
                    stop = true;
                }
                else {
                    isSeeing = false;
                }
            }
            else {
                isSeeing = false;

            }
        }
        else {
            visionsOfGround = visionGround.transform.gameObject; ; //Variavel pra confirmar se o que o drone detectou é uma parede
            isSeeing = false;

        }
        //Timer que cria um intervalo de tempo entre a perda de visão do player e a volta ao MoveDefault        
        if (!isSeeing && wasSeen || !range && wasSeen) { 
            if (stop) {
                angleTime = UnityEngine.Time.time + angleRate; 
                isGotOut = true;
                stop = false;

            }
            if (isGotOut) {
                transform.eulerAngles = new Vector3(0, 0, angle);
                rb.velocity = Vector2.zero;
                if (UnityEngine.Time.time > angleTime) {
                    isGotOut = false;
                }
            }
            else {
                transform.eulerAngles = new Vector3(0, 0, 0);
                rb.velocity = new Vector2(speedDefault, 0);
            }
        } 
    }
        

    private void FollowPlayer() { //Faz o inimigo seguir o player
        Vector2 posicaoAlvo = player.position;                                                  
        Vector2 posicaoAtual = transform.position;                                             
        distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);
        Vector2 direcao = posicaoAlvo - posicaoAtual;             
        Vector3 direcaoX = new Vector2 (direcao.normalized.x, 0); //Posição apenas do eixo X do player
            // se a distancia do player for maior que a distanciaMin o Drone ira se mover em direção ao eixo X do player
            if (distancia >= distanciaMin) {
                rb.velocity = direcaoX * speed;
                Shoot();
            }
            else {
                rb.velocity = Vector2.zero; //Se a distancia for menor que a mínima o inimigo vai ficar parado
            }
            if (rb.velocity.x != 0) { //Se o inimigo estiver se movendo
                anim.SetBool("Front", true); //Inicia a animação de movimentação
            }
            else {
                anim.SetBool("Front", false);
            }
    }

    private void Angle(){ //Calcula o angulo do player para drone
        if (isSeeing && range) {
            //Rotacao
            Vector2 posicaoAlvo = player.position;
        Vector2 posicaoAtual = transform.position;
        float distancia = Vector2.Distance(posicaoAtual, posicaoAlvo);
        Vector2 direcao = posicaoAlvo - posicaoAtual;
        var dir = direcao; //variavel auxiliar do angle
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; //o angle é igual ao angulo x e y do player em relação ao drone

            transform.eulerAngles = new Vector3(0, 0, angle); //torna o eixo z igual ao angle
        }
        //Girar
        if (isGotOut) {
            Vector3 localScale = Vector3.one; //Se o player saiu do raio de visão o Drone irá ficar perfeitamente reto

            if (angle > 90 || angle < -90) {
                localScale.y = -1f; //Se o player estiver atrás do Drone ele irá virar pra trás
            }
            else {
                localScale.y = 1f; //Se não vai ficar virado para frente
            }
       
            transform.localScale = localScale; //Atribui as variaveis a cima ao componente do Drone

        }

    }
    private void OnDrawGizmos() { //Desenha as formas geometricas das colisões para serem vistas no editar da unity
        Gizmos.DrawWireSphere(transform.position, radios);
      //  Gizmos.DrawLine(visaoObj.position, player.position);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(wallOffSett, 0), wallRadius);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-wallOffSett, 0), wallRadius);
    }
    private void Shoot() { //Realiza um disparo 
        if (UnityEngine.Time.time  > nextShoot) { //Timer do intervalo entre cada tiro
            nextShoot = UnityEngine.Time.time + shootRate; //Define o tempo do próximo tiro
            Instantiate(bullet, barrel.position, barrel.rotation); //Gera o tiro

        }

    }
    public void RecebeDano(int dano) { 
        vida -= dano;

        if (vida <= 0) { Destroy(gameObject); }
    }
   

    void Flip() {
        FlipX = !FlipX;
         Flipx = transform.localScale.x;
        Flipx *= -1;
        transform.localScale = new Vector3(Flipx, transform.localScale.y, transform.localScale.z);
    
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            range = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") {
            RecebeDano(5); 

        }
        if (collision.gameObject.tag == "Espada") {
            RecebeDano(5);
        }
        if (collision.gameObject.tag == "Player") {
            range = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
             range = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
           // range = false;
        }
    }
    void Classes() {
        isHidding = FindAnyObjectByType<PlayerController>().isHidding; //isHidding do Drone recebe o valor de isHidding do player
    }
}

