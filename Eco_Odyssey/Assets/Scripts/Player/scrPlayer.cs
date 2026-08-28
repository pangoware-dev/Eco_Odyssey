using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class scrPlayer : MonoBehaviour{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 input;

    public Animator anim;

    public RuntimeAnimatorController animControllerNormal;

    public int PlayerMode = 0;

    public LayerMask enemyLayer;

    public scrAnimationControl ac;
    public CircleCollider2D playerCollider;
    private scrLife life;


    // KNOCKBACK
    private bool isKnockedBack;


    // GLOBAL STATUS
    private scrGlobalStatus globalStatus;


    // STATUS DO JOGADOR
    private float normalSpeed = 6.25f;

    private float normalHealth;

    private float normalCurrentHealth;

    private float ecoSpeed;

    private bool usingEco = false;


    // UPDATE
    private void Update(){
        if (Input.GetButtonDown("Slash") && PlayerMode == 1){
            ac.Attack();
        }
        else if (Input.GetButtonDown("Slash") && PlayerMode == 0){
            //Interact();
        }


        if (anim.GetBool("isAttacking") == false){
            input.x = Input.GetAxisRaw("Horizontal");

            input.y =Input.GetAxisRaw("Vertical");
        }
        else{
            input.x = 0;
            input.y = 0;
        }


        input.Normalize();
        PlayerModeChange();
    }

    public void PlayerModeChange()
    {
        if (PlayerMode==1)
        {
            life.SetHealthBarVisible();
            playerCollider.offset = new Vector2(0f, 0f);
        }
        else
        {
            life.SetHealthBarInvisible();
            playerCollider.offset = new Vector2(0f, -0.5f);
        }
    }
    


    // START
    void Start(){
        rb = GetComponent<Rigidbody2D>();

        globalStatus = GetComponent<scrGlobalStatus>();

        ac = GetComponent<scrAnimationControl>();

        speed = normalSpeed;


        life = GetComponent<scrLife>();

        normalHealth = life.MaxHealth;

        normalCurrentHealth = life.CurrentHealth;
    }


    // FIXED UPDATE
    private void FixedUpdate(){
        if (!isKnockedBack){
            rb.linearVelocity = new Vector2(input.x, input.y) * speed;

            anim.SetFloat("horizontal", input.x);

            anim.SetFloat("vertical", input.y);
        }


        // MODO NORMAL
       if (PlayerMode == 0){
            scrLife life = GetComponent<scrLife>();

            speed = normalSpeed;

            life.MaxHealth =  normalHealth;

            life.CurrentHealth = normalCurrentHealth;

            anim.runtimeAnimatorController =  animControllerNormal;

            // Cura todos os Ecos ao sair da batalha
            if (usingEco){
                globalStatus.HealParty();
            }

            usingEco = false;
        }


        // MODO ECO
        else
        {
            TransformEco();
        }


        EnemyRange();

        CheckEcoSwitch();
    }


    // DETECTAR INIMIGO
   private void EnemyRange(){
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, 5f, enemyLayer);

        if (enemy != null)
        {
            PlayerMode=1;
            //life.SetHealthBarVisible();
        }
        else
        {
            PlayerMode=0;
            //life.SetHealthBarInvisible();
        }
    }


    // KNOCKBACK
    public void Knockback(Transform enemy, float force, float stunTime){
        isKnockedBack = true;

        Vector2 direction = (transform.position - enemy.position).normalized;


        rb.linearVelocity = direction * force;


        StartCoroutine(KnockbackCounter(stunTime));
    }


    IEnumerator KnockbackCounter(float stunTime){
        yield return new WaitForSeconds(stunTime);

        rb.linearVelocity = Vector2.zero;

        isKnockedBack = false;
    }


    // TRANSFORMAR EM ECO
   void TransformEco(){
        if (!usingEco){
            ApplyEcoStats();
            usingEco = true;
        }
    }


    // TROCA MANUAL DE ECO
   void CheckEcoSwitch(){
        if (!usingEco){
            return;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1)){
            SwitchEco(0);
        }


        if (Input.GetKeyDown(KeyCode.Alpha2)){
            SwitchEco(1);
        }


        if (Input.GetKeyDown(KeyCode.Alpha3)){
            SwitchEco(2);
        }


        if (Input.GetKeyDown(KeyCode.Alpha4)){
            SwitchEco(3);
        }


        if (Input.GetKeyDown(KeyCode.Alpha5)){
            SwitchEco(4);
        }


        if (Input.GetKeyDown(KeyCode.Alpha6)){
            SwitchEco(5);
        }


        if (Input.GetKeyDown(KeyCode.Alpha7)){
            SwitchEco(6);
        }
    }


    // TROCAR ECO
   void SwitchEco(int index){
        if (globalStatus.EquipEco(index)){
            ApplyEcoStats();
        }
    }


    // APLICAR STATUS DO ECO
    void ApplyEcoStats(){
        scrLife life = GetComponent<scrLife>();

        ecoSpeed = globalStatus.veloC;

        speed = (ecoSpeed + 15 * Mathf.Sqrt( globalStatus.levelC)) / 10;

        life.MaxHealth = globalStatus.vidaC;

        life.CurrentHealth = globalStatus.vidaAtual;

        life.UpdateHPText();

        anim.runtimeAnimatorController = globalStatus.animControllerC;
    }


    // ECO MORREU
    public void EcoDied(){
        Debug.Log("O Eco atual morreu!");


        int nextEco = globalStatus.GetNextAliveEco();


        // TODOS OS ECOS MORRERAM
        if (nextEco == -1){
            Debug.Log("Todos os Ecos morreram!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

/*             PlayerMode = 0;

            usingEco = false;

            return; */
        }


        // ENCONTRADO PRÓXIMO ECO
       Debug.Log("Chamando automaticamente o Eco " + "do slot " + (nextEco + 1));


        if (globalStatus.EquipEco(nextEco)){
            ApplyEcoStats();
            usingEco = true;
        }
    }
}