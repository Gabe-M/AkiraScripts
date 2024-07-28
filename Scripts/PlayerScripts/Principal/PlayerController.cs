using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour {
     [SerializeField] GameObject menuObj;   //Verifica o objeto Menu
    [SerializeField] public int vida;  
    [SerializeField] GameObject[] heartUI;  //Sprites de corações da interface
    private Rigidbody2D rb;  //Física do player
    private DIalogoController dialogoController;     //Dialogos dos NPCS
    private Animator anim;  //Animação
    private BoxCollider2D boxCollider;   //Colisão do Player
    [SerializeField] private LayerMask bulletLayer;     //Camada dos tiros
    [SerializeField] private LayerMask enemyLayer;   //Camada do inimigo
    [SerializeField] private GameObject arm;     //Braço do player
    [SerializeField] private GameObject playerHolder;   //Objeto que segura a arma
    [Header("Move")]
    #region Move
    public float speed = 4;    
    private float horizontalMovement;   //Define o movimento horizontal do player
    private bool FlipX;   //Verifica se o eixo X do player está positivo ou negativo
    private bool canMove;  //Define se o player pode andar
    public bool run = true;    
    public bool walk = false; 
    public bool isDown; 
    public bool isHidding = false; //Define que o player está escondido
    [SerializeField] bool hide = false; //Verifica se o player pode se esconder
    #endregion
    [Header("Jump")]
    #region Jump

    [SerializeField] private float jumpForce; //Força do pulo
    public bool isJumping = false; //Verifica se o player está pulando
    [SerializeField] private bool doubleJump; //Verifica se está em um salto duplo
    public Transform groundCheck; //Verifica se o player está no chão
    [SerializeField] public LayerMask groundLayer; //Camada do chão
    [SerializeField] bool inGround; //Verifica se o player está no chão
    #endregion
    [Header("Dash")]
    #region Dash
    [SerializeField] private float dashingPower; //Força do dash
    [SerializeField] private float dashingTime; //Duração do dash
    [SerializeField] private float dashingCoolDown; //Tempo de recarga do dash
    [SerializeField] TrailRenderer tr; //Classe que cria um rastro por onde o player passa
     private float fallTime; //Tempo que o player está caindo
    private bool canDash = true; //Verifica se pode dar um dash
    private bool isDashing; //Verifica se está realizando um dash

    #endregion
    [Header("WallJump")]
    #region Wall
    [SerializeField] private float WallRadius; //Tamanho da colisão na parede
    [SerializeField] private LayerMask wallLayer; //Camada da parede
    [SerializeField] private Vector3 wallOffSett; //Afasta o ponto central que verifica a parede do player
    [SerializeField] private bool onWall = false; //Verifica se o player está encostando em uma parede
    [SerializeField] private float maxFallSpeed; //Velocidade maxima que o player pode cair
    [SerializeField] private float WallJumpForce; //Força que o player pula de uma parede
    [SerializeField] bool rightWall = false; //Verifica se tem uma parede na direita
    [SerializeField] bool leftWall = false; //Verifica se tem uma parede na esquerda
    [SerializeField] private bool isFall; //Verifica se o player está caindo
    [SerializeField] private bool returnJump = false; //Verifica se pode pular novamente
    [SerializeField] private float fallRate; //Tempo que o player pode cair em uma parede
    private float WallJumpDuration; 
    private bool JumpForWall; //Verifica se o Player pulou de uma parede
    private bool canReturnJump = false; //Define se ele pode pular
    private bool isHang = false; //Verifica se está agarrado em uma parede
    #endregion
    [Header("Item")]
    #region
    public bool isKey;
    #endregion
    
 
    void Start() {     //Função que roda quando inicia o jogo
    //Pega as variaveis e as torna componentes do player
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
      
      
    }

    void Update() { //Função que roda a cada frame por segundo (FPS)
        if(menuObj.activeSelf == false) {    //Está função só roda se o menu estiver fechado
        Animation(); 
        WallJump();
        CheckGrounded();
        SitDown();
        Sla();
        
        if (isDashing) { 
            return;     //Se o player está dando um dash o Uptade termina aqui
        }
        if (Input.GetKeyDown(KeyCode.F) && canDash == true) {  //Se a tecla F for apertada enquanto pode dar um dash
            anim.SetTrigger("Dash");    //Roda a animação do player dando o dash
            StartCoroutine(Dash());     //Chama o IEnumerator dash
        }
        if(vida <= 0){
            Death(); 
        }
        }
    }

    private void FixedUpdate() { //Essa função roda 60 vezes em um segundo (Melhor para funções que precisam rodar em um numero fixo)

        if (isDashing) { //Se o player está dando um dash a função acaba aqui
            return;
        } 
        if(menuObj.activeSelf == false) {  Mover(); } //O player apenas pode se mover com o menu desativado
        
    }

    void Mover() {
            horizontalMovement = Input.GetAxisRaw("Horizontal"); //Uma função unica da unity que torna um float um numero de 0 a 1 que define o eixo X que o player ira se mover
            Vector3 playerToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //Cria um vetor de 3 direções baseado na posição do mouse com a do player
            playerToMouseDir.z = 0; //zera o eixo z do vetor

        if (!isDown) {
            rb.velocity = new Vector2(horizontalMovement * speed, rb.velocity.y); 
        }else {
            rb.velocity = new Vector2(horizontalMovement * 3, rb.velocity.y); //Se o player estiver agachado a velocidade diminuira
        } if(!isHang){
            if (FlipX == false && playerToMouseDir.x < 0) { //Se a posição do mouse for menor que 0 e o player não fez nenhum flip então ele flipa
                Flip();
            }
            if (FlipX == true && playerToMouseDir.x > 0) {
                Flip();
             }
        }
        
}

    void CheckGrounded() {
        inGround = Physics2D.OverlapCircle(groundCheck.position, 0.45f, groundLayer); //Cria uma colisão circular que detecta o chão e o resultado vai para o inGround
        if (inGround == true || onWall) {
            doubleJump = false;
               returnJump = false;
               
            if (Input.GetButtonDown("Jump")) { //É uma função unica da unity que retorna verdadeiro ao apertar Espaço
                Jumping();
                doubleJump = false;
                isJumping = true;
                returnJump = false;
            }
        } else if (inGround == false & !onWall) {
            if (Input.GetButtonDown("Jump")) {
                Jumping();
                anim.SetTrigger("DoubleJump"); //Inicia a animação do doubleJump
                doubleJump = true;       
                isJumping = false;
                canReturnJump = true;
                 fallTime = UnityEngine.Time.time + fallRate; //fallTime se torna igual ao tempo atual mais o fallRate
                
              
            }
              if (UnityEngine.Time.time > fallTime && canReturnJump) { //É um timer que termina quando o tempo atual for maior que o fallTime
                    returnJump = true;
                    canReturnJump = false;
            }
        }
        if(!onWall && !inGround && !isJumping && !doubleJump){
            isFall = true;
        }else{
            isFall = false;
        }
    }
   
    void Jumping() {
        if (inGround || !doubleJump || onWall) {
            rb.velocity = new Vector2(speed, 0); //A velocidade do eixo x se torna igual ao speed
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); //Cria um impulso no player assim o fazendo pular
          
        } else if (onWall == true && inGround) {
            canMove = false;
            JumpForWall = true;
            Flip();
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(WallJumpForce, jumpForce), ForceMode2D.Impulse);
        }
        if(!JumpForWall && !canMove) {
            if (Input.GetAxisRaw("Horizontal") != 0 || inGround) { 
                canMove = true;
            }

        }
    }

 
    IEnumerator Dash() { //Realiza um dash
        canDash = false; //Impede que o player realize diversos dash de uma vez
        isDashing = true; //Define que o player esta em um dash
        rb.excludeLayers = bulletLayer; //Faz o player ignorar a camada dos tiros
        float originalGravity = rb.gravityScale; //Armazena o valor da gravidade antes do dash
        rb.gravityScale = 0f; //Zera a gravidade enquanto dura o dash
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f); //Realiza o dash para frente
        tr.emitting = true; //Ativa o rastro
        yield return new WaitForSeconds(dashingTime); //Pausa a função até o dash terminar
        tr.emitting = false; // Desativa o rastro
        rb.gravityScale = originalGravity; //Volta com a gravidade original
        isDashing = false; // Define que o player não esta em dash
        rb.excludeLayers = 0; //Volta a colidir com a camada dos tiros
        yield return new WaitForSeconds(dashingCoolDown); //Espera o dashingCoolDown para continuar
        canDash = true; //Permite que o player dê outro dash
    }
    void Flip() { //Inverte o eixo X do player 
        FlipX = !FlipX; //Inverte a variavel FlipX
        float x = transform.localScale.x; //x é igual ao eixo x do player
        x *= -1; //inverte o valor de x
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z); //coloca x como o novo eixo X do player
    }

    void WallJump(){
        onWall = false;
            //Cria duas colisões que detectam paredes
        rightWall = Physics2D.OverlapCircle(transform.position + new Vector3(wallOffSett.x, 0), WallRadius, wallLayer);
        leftWall = Physics2D.OverlapCircle(transform.position + new Vector3(-wallOffSett.x, 0), WallRadius, wallLayer);
            //Olha para a direção da parede que o player colide
        if(rightWall) {
           if(transform.localScale.x < 0){ 
            Flip();
           }
           onWall = true;
        }if(leftWall) {
           if(transform.localScale.x > 0){
            Flip();
           }
           onWall = true;
        }
        //faz com que o player pare de cair ao alcançar o maxFallSpeed
        if(onWall) {
            if(rb.velocity.y < maxFallSpeed) { 
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                doubleJump = false;
            }
        }
    }

    void SitDown() {
        if (Input.GetKeyDown(KeyCode.LeftControl)) { //ao apertar a tecla CTRL
            if (!walk) {
                speed = 4;
                walk = true;
                run = false;
            }
            else if (!run) {
                speed = 8;
                walk = false;
                run = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift)) { //Ao apertar a tecla Shift
            if (!isDown && inGround) {
                transform.position = new Vector3(transform.position.x, transform.position.y, 19); //Aumenta o eixo z
                isDown = true;
            }else {
                isDown = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, -1);//Diminui o eixo z
            }
        }
        if (inGround || isDashing) {
            isDown = false;
        }
        if(isDown && hide) {
            isHidding = true;

        }
        else {
            isHidding = false;
        }
            anim.SetBool("SitDown", isDown);

        if (isHidding) {
            rb.excludeLayers = enemyLayer;
        }
        else {
            rb.excludeLayers = 0;
        }
    }

    

    private void OnDrawGizmos() {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(groundCheck.position, 0.45f);
         //Desenha uma esfera do tamanho da colisão no editor da unity
        Gizmos.DrawWireSphere(transform.position + new Vector3(wallOffSett.x, 0), WallRadius); //Desenha uma esfera do tamanho da colisão no editor da unity
        Gizmos.DrawWireSphere(transform.position + new Vector3(-wallOffSett.x, 0), WallRadius);
    }
    public void Animation() { //ativa e desativa algumas animações quando se atinge certas condições
        anim.SetFloat("Speed", speed);
        if (horizontalMovement != 0 && inGround) { //se o player está se movimentando no chão
            anim.SetBool("Move", true);
        }
        else {
            anim.SetBool("Move", false);
        }
        if (isJumping && !inGround && !onWall || doubleJump && !onWall) {
            anim.SetBool("Jump", true);
        }
        else {
            anim.SetBool("Jump", false);
        }
        if (onWall && !inGround) {
            anim.SetBool("Hang", true);
            isHang = true;
        }else{
            anim.SetBool("Hang", false); 
             isHang = false;
        }
        if(doubleJump && !returnJump || isDown || isDashing || onWall && !inGround || isFall){
            arm.SetActive(false);
            playerHolder.SetActive(false); //desativa o objeto holder
        }else{
            arm.SetActive(true);
            playerHolder.SetActive(true); //desativa o objeto holder
        }
        if(isFall){
            anim.SetBool("isFall", true);
        }else{
            anim.SetBool("isFall", false);  
        }
        
    }
    
    public void RecebeDano(int dano) {
        vida -= dano;
    }

    void Death(){
        SceneManager.LoadScene("Tutorial"); //reinicia o jogo
       
    }
    void Sla(){
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Tutorial");
        }
        #region LifeUI 
        //define a quantidade de corações que irá aparecer na interface
        if(vida >= 3){
            heartUI[0].SetActive(true);
            heartUI[1].SetActive(true);
            heartUI[2].SetActive(true);
        }else if(vida == 2){
            heartUI[0].SetActive(true);
            heartUI[1].SetActive(true);
            heartUI[2].SetActive(false);
        }else if(vida == 1){
            heartUI[0].SetActive(true);
            heartUI[1].SetActive(false);
            heartUI[2].SetActive(false);
        }else if(vida <= 0){
            heartUI[0].SetActive(false);
            heartUI[1].SetActive(false);
            heartUI[2].SetActive(false);
        }
        #endregion
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Key")) { //verifica se colidiu com um objeto com a tag Key
            isKey = true;
            Destroy(collision.gameObject); //Destroi o objeto que o player colidiu
        }
        if(collision.gameObject.CompareTag("CheckPoint")){ //Mesma coisa com a tag Checkpoint
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Hide")) { //Verifica se entrou em uma colisão de uma tag Hide
            hide = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Hide")) { //Verifica se saiu de uma colisão de uma tag Hide
            hide = false;
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("DoorClosed") && isKey) { //Verifica se colidiu com uma tag DoorClosed
            isKey = false;
            Destroy(collision.gameObject); //Destroi o objeto que o player colidiu 
        }
        if(collision.gameObject.CompareTag("Bullet")){ //Veridica se colidiu com um tiro
            RecebeDano(1); //Recebe 1 de dano
        }
    }

}
