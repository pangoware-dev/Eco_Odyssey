using UnityEngine;

public class scrAnimationControl : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    private scrGlobalStatus globalStatus;
    public Transform attackPoint;
    public int damage;
    public float weaponRange;
    public float knockbackForce = 10f;
    public LayerMask playerLayer;
    public float stunTime = 0.5f;
    private scrEnemyEco ecoComp;

    public float fakeRange = 1f;
    public LayerMask enemyLayer;
    public int Ataque;
    public float cooldown = 1f;
    private float timer;
    private Animator anim;
    
    
    void Start()
    {
        globalStatus = GetComponent<scrGlobalStatus>();
        // Detecta automaticamente se é Player pela tag
        isPlayer = CompareTag("Player");
        // Encontra o AttackPoint se não foi atribuído
        if (attackPoint == null)
        {
            attackPoint = transform.Find("AttackPoint");
            if (attackPoint == null)
            {
                Debug.LogWarning($"AttackPoint não encontrado em {gameObject.name}");
            }
        }
        
        ecoComp = GetComponent<scrEnemyEco>();
        anim = GetComponent<Animator>();

        if (ecoComp != null && ecoComp.ecoData != null)
        {
            damage = (int)ecoComp.ecoData.Ataque;
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        EnemyDodge();
    }

    public void FixedUpdate()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (globalStatus != null)
            {
                Ataque = (int)globalStatus.atkC;
            }
        }
    }
    
    public void Attack()
    {
        //É NPC?
        if (gameObject.CompareTag("Enemy"))
        {
            if (attackPoint == null) return;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

            if (hits.Length > 0)
            {
                // Verifica se tem o componente scrLife
                scrLife life = hits[0].GetComponent<scrLife>();
                if (life != null)
                {
                    if (ecoComp != null && ecoComp.ecoData != null)
                    {
                        life.ChangeHealth(damage, ecoComp.ecoData);
                        Debug.Log("Eco Inimigo: "+ecoComp.ecoData.name);
                    }
                }
                
                // Verifica se tem o componente scrPlayer
                scrPlayer player = hits[0].GetComponent<scrPlayer>();
                if (player != null)
                {
                    player.Knockback(transform, knockbackForce, stunTime);
                }
            }
        }

        //É o Player?
        else if (gameObject.CompareTag("Player"))
        {
            if (anim == null) return;
            
            anim.SetBool("isAttacking", true);
            
            if (attackPoint == null) return;
            
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

            if (enemies.Length > 0)
            {
                // Verifica se tem o componente scrEHealth
                scrEHealth eHealth = enemies[0].GetComponent<scrEHealth>();
                if (eHealth != null)
                {
                    if (globalStatus != null && globalStatus.currentEco != null)
                    {
                        eHealth.changeHP(Ataque, globalStatus.currentEco);
                        Debug.Log("Eco Player: "+globalStatus.currentEco);
                    }
                }
                
                // Verifica se tem o componente scrEnemyKB
                scrEnemyKB enemyKB = enemies[0].GetComponent<scrEnemyKB>();
                if (enemyKB != null)
                {
                    enemyKB.Knockback(transform, knockbackForce, stunTime);
                }
            }
        }
    }

    public void EnemyDodge()
    {
        if (attackPoint == null || anim == null) return;
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, fakeRange, enemyLayer);

        if (enemies.Length > 0 && anim.GetBool("isAttacking") == true)
        {
            scrChase chase = enemies[0].GetComponent<scrChase>();
            if (chase != null)
            {
                chase.canDodge(1);
            }
        }
    }

    public void AttackUp()
    {
        if (attackPoint == null) return;
        attackPoint.position = new Vector2(transform.position.x, transform.position.y + 0.3f);
    }

    public void AttackDown()
    {
        if (attackPoint == null) return;
        attackPoint.position = new Vector2(transform.position.x, transform.position.y - 0.3f);
    }

    public void AttackLeft()
    {
        if (attackPoint == null) return;
        attackPoint.position = new Vector2(transform.position.x - 0.3f, transform.position.y);
    }

    public void AttackRight()
    {
        if (attackPoint == null) return;
        attackPoint.position = new Vector2(transform.position.x + 0.3f, transform.position.y);
    }

    public void EndAttackAnimation()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null) return;
        }
        anim.SetBool("isAttacking", false);
    }

    // Método auxiliar para visualizar as áreas de ataque na cena (opcional)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, fakeRange);
        }
    }
}