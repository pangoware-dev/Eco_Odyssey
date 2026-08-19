using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NPC_Talking : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator InteractAnim;
    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;
    private scrPlayer player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<scrPlayer>();
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

    private void Update()
    {
    if (Input.GetButtonDown("Slash") && player.PlayerMode == 0)
    {
        if (DialogueManager.Instance.isDialogueActive)
        {
            DialogueManager.Instance.AdvanceDialogue();
        }
        else
        {
            CheckForNewConversation();

            if (currentConversation != null)
            {
                DialogueManager.Instance.StartDialogue(currentConversation);
            }
        }
    }
}

    private void CheckForNewConversation()
    {
        for (int i = conversations.Count - 1; i >= 0; i--)
        {
            var convo = conversations[i];

            if (convo != null && convo.isConditionMet())
            {
                currentConversation = convo;
                conversations.RemoveAt(i);
                break;
            }
        }
    }
}
