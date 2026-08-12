using UnityEngine;

public class NPC_Talking : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator InteractAnim;
    public DialogueSO dialogueSO;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.Play("Idle");
        InteractAnim.Play("Open");
    }

    private void OnDisable()
    {
        InteractAnim.Play("Close");
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<scrPlayer>().SetDialogue(dialogueSO);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<scrPlayer>().SetDialogue(null);
        }
    }

    /* private void Update()
    {
        if (Input.GetButtonDown("Slash"))
        {
            if(DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else
            {
                DialogueManager.Instance.StartDialogue(dialogueSO);
            }
        }
    } */
}
