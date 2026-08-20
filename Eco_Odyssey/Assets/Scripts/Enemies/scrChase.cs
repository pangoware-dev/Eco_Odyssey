using UnityEngine;

public class scrChase : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    public Animator anim;
    private Animator playerAnim;
    private EnemyState enemyState;

    private scrEnemyEco ecoComp;

    public float attackRange = 2.1f;
    private int inDodgeRange=0;
    public float attackCooldown = 1;
    private float attackCooldownTimer;
    public float speed;
    public float ecoSpeed;
    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;
    private float playerx=0, playery=0;
    private scrEHealth health;
    public scrGlobalStatus globalStatus;
    public int Level=5;
    Vector2 direction=Vector2.zero;

    void Start()
    {
        ecoComp = GetComponent<scrEnemyEco>();
        health = GetComponent<scrEHealth>();

        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
        ApplyStatus();
}

    private void ApplyStatus()
    {
        //Velocidade
        if (ecoComp != null && ecoComp.ecoData != null)
        {
            ecoSpeed = ecoComp.ecoData.Velocidade;
        }
    }

    void FixedUpdate()
    {
        if(enemyState!=EnemyState.Knockback){
            CheckForPlayer();
            if(attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            if (enemyState == EnemyState.Moving)
            {
                Chase();
            }
            else if(enemyState == EnemyState.Attacking)
            {
                rb.linearVelocity = Vector2.zero;
            }
            
            if (anim.GetBool("isAttacking")==false)
            {
                ChangeState(EnemyState.Moving);
            }
        }
        Level=globalStatus.levelC;
    }

    public void canDodge(int dodge)
    {
        inDodgeRange=dodge;
    }

    public void Chase()
    {
        ApplyStatus();
        playerx=player.position.x-transform.position.x;
        playery=player.position.y-transform.position.y;
        float absX = Mathf.Abs(playerx);
        float absY = Mathf.Abs(playery);
        
        if (absX > absY)
        {
            // Movimento horizontal dominante
            anim.SetFloat("horizontal", playerx > 0 ? 1 : -1);
            anim.SetFloat("vertical", 0);
        }
        
        else if (absY > absX)
        {
            // Movimento vertical dominante
            anim.SetFloat("vertical", playery > 0 ? 1 : -1);
            anim.SetFloat("horizontal", 0);
        }
        else
        {
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
        }

        
        if (inDodgeRange==0)
        {
            direction=(player.position-transform.position).normalized;
        } else{

            if (playerAnim.GetBool("isAttacking")==false)
            {
                inDodgeRange=0;
            }
            direction=(transform.position-player.position).normalized;
            }
            speed = (ecoSpeed+2*Mathf.Sqrt(Level))/5;
            rb.linearVelocity=direction*speed;
    }

    
        public void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if(hits.Length > 0)
        {
            player = hits [0].transform;
            playerAnim = player.GetComponent<Animator>();

                if(Vector2.Distance(transform.position, player.transform.position) <= attackRange && attackCooldownTimer <= 0)
                {
                    attackCooldownTimer =  attackCooldown;
                    ChangeState(EnemyState.Attacking);
                }

                else if(Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
                {
                    ChangeState(EnemyState.Moving);
                    health.SetHealthBarVisible();
                }
        }

        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
            anim.SetBool("isIdle", true);
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
            health.SetHealthBarInvisible();
        }
    }



    public void ChangeState(EnemyState newState)
    {
        //Parar estado atual
        if (enemyState==EnemyState.Idle)
        {
            anim.SetBool("isIdle", false);
        } else if (enemyState==EnemyState.Moving)
        {
            anim.SetBool("isMoving", false);
        } else if (enemyState==EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", false);
        }

        //Transicionar para novo estado
        enemyState=newState;

        //Iniciar novo estado
        if (enemyState==EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
        } else if (enemyState==EnemyState.Moving)
        {
            anim.SetBool("isMoving", true);
        } else if (enemyState==EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", true);
        }
    }

    //visualizar o Range Distance do inimigo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
        Gizmos.DrawWireSphere(detectionPoint.position, attackRange);
    }

}


public enum EnemyState
{
    Idle,
    Moving,
    Attacking,
    Knockback
}