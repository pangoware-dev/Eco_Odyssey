using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DialogueHistoryTracker : MonoBehaviour
{
    public static DialogueHistoryTracker Instance;
    private readonly HashSet<ActorSO> spokenNPCs = new HashSet<ActorSO>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);
        Debug.Log("Falou com o: "+actorSO.actorName);
    }

    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);
    }
}
