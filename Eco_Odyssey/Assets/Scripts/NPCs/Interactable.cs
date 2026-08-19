using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Interactables : MonoBehaviour
{
    public Animator InteractAnim;
    public DialogueSO currentConversation;
    public bool isInteractable = false;
    public List<DialogueSO> conversations;
    private scrPlayer player;

    private void Start()
    {
        InteractAnim.Play("Idle");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<scrPlayer>();
    }
    private void Open()
    {
        InteractAnim.Play("Open");
    }

    private void Close()
    {
        InteractAnim.Play("Close");
    }

     private void Update()
    {
        if (Input.GetButtonDown("Slash"))
        {
            if (isInteractable==true&&player.PlayerMode==0)
            {
                if(DialogueManager.Instance.isDialogueActive)
                {
                    DialogueManager.Instance.AdvanceDialogue();
                }
                else
                {
                    CheckForNewConversation();
                    DialogueManager.Instance.StartDialogue(currentConversation);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Open();
            isInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Close();
            isInteractable = false;
        }
    }

    private void CheckForNewConversation()
    {
        for (int i=conversations.Count -1; i>=0; i++)
        {
            var convo = conversations[i];
            if(convo != null && convo.isConditionMet())
            {
                conversations.RemoveAt(i);
                currentConversation=convo;
            }
        }
    }
}
