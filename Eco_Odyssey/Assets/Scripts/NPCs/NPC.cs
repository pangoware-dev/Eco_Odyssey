using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPCState {Default, Patrol, Idle, Talking};
    public NPCState currentState=NPCState.Patrol;
    private NPCState defaultState;
    public NPC_Patrol patrol;
    public NPC_Talking talking;

    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {
        currentState = newState;
        patrol.enabled = newState == NPCState.Patrol;
        talking.enabled = newState == NPCState.Talking;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SwitchState(NPCState.Talking);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SwitchState(defaultState);
        }
    }
}
