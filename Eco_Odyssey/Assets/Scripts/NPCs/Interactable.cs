using UnityEngine;

public class Interactables : MonoBehaviour
{
    public Animator InteractAnim;
    public DialogueSO dialogueSO;
    public bool isInteractable = false;

    private void Start()
    {
        InteractAnim.Play("Idle");
    }
    private void Open()
    {
        InteractAnim.Play("Open");
    }

    private void Close()
    {
        InteractAnim.Play("Close");
    }

/*     private void Update()
    {
        if (Input.GetButtonDown("Slash"))
        {
            if (isInteractable==true)
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
        }
    } */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Open();
            isInteractable = true;
            collision.GetComponent<scrPlayer>().SetDialogue(dialogueSO);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Close();
            isInteractable = false;
            collision.GetComponent<scrPlayer>().SetDialogue(null);
        }
    }
}
