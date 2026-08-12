using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scrPlayer : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 input;
    public Animator anim;
    public RuntimeAnimatorController animControllerNormal;
    public int PlayerMode=0;
    public LayerMask enemyLayer;
    public scrAnimationControl ac;
    public DialogueSO currentDialogue;

    //public scrPCombat player_Combat;
    private bool isKnockedBack;

    private scrGlobalStatus globalStatus;

    private float normalSpeed=6.25f;
    private float normalHealth;
    private float normalCurrentHealth;
    private float ecoSpeed;

    private bool usingEco = false;
    
    private void Update()
    {
        if (Input.GetButtonDown("Slash")&&PlayerMode==1)
        {
            ac.Attack();
        } else if (Input.GetButtonDown("Slash") && PlayerMode == 0)
        {
            Interact();
        }
        if(anim.GetBool("isAttacking")==false)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        } else
        {
            input.x = 0;
            input.y = 0;
        }
        input.Normalize();
    }

    public void SetDialogue(DialogueSO dialogueSO)
    {
        currentDialogue = dialogueSO;
    }

    public void Interact()
    {
            if(DialogueManager.Instance.isDialogueActive && currentDialogue != null)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else if (currentDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(currentDialogue);
            }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        globalStatus = GetComponent<scrGlobalStatus>();
        ac = GetComponent<scrAnimationControl>();

        speed = normalSpeed;

        normalHealth = GetComponent<scrLife>().MaxHealth;
        normalCurrentHealth = GetComponent<scrLife>().CurrentHealth;
    }

    private void FixedUpdate()
    {
        if (isKnockedBack==false)
        {
            rb.linearVelocity = new Vector2(input.x, input.y) * speed;
            anim.SetFloat("horizontal", input.x);
            anim.SetFloat("vertical", input.y);
        }
        if (PlayerMode == 0)
        {
            scrLife life = GetComponent<scrLife>();
            speed = normalSpeed;
            life.MaxHealth = normalHealth;
            life.CurrentHealth = normalCurrentHealth;
            anim.runtimeAnimatorController = animControllerNormal;
            usingEco = false;
        }
        else
        {
            TransformEco();
        }

        EnemyRange();
        CheckEcoSwitch();
    }
    private void EnemyRange()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, 5f, enemyLayer);

        if (enemy != null)
        {
            PlayerMode=1;
        }
        else
        {
            PlayerMode=0;
        }
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity=direction*force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    void TransformEco()
    {
        if (!usingEco)
        {
            ApplyEcoStats();

            usingEco = true;
        }
    }

    void CheckEcoSwitch()
    {
        // Só pode trocar se estiver transformado
        if (!usingEco)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchEco(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchEco(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchEco(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchEco(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchEco(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchEco(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchEco(6);
        }
    }

    void SwitchEco(int index)
    {
        globalStatus.EquipEco(index);

        if (usingEco)
        {
            ApplyEcoStats();
        }
    }

    void ApplyEcoStats()
    {
        scrLife life = GetComponent<scrLife>();

        ecoSpeed = globalStatus.veloC;
        speed = (ecoSpeed+15*Mathf.Sqrt(globalStatus.levelC))/10;

        life.MaxHealth = (int)globalStatus.vidaC;
        life.CurrentHealth = life.MaxHealth;

        life.HPText.text =
            "HP: " + life.CurrentHealth + "/" + life.MaxHealth;

       anim.runtimeAnimatorController = globalStatus.animControllerC;
    }
}