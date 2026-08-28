using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Requerimentos condicionais (Opcional)")]
    public ActorSO[] requiredNPCs;
    public LocationSO[] requiredLocations;
    //public ItemsSO[] requiredItems;

    public bool isConditionMet()
    {
        //Checar NPCs
        if (requiredNPCs.Length>0)
        {
            foreach (var npc in requiredNPCs)
            {
                if (!DialogueHistoryTracker.Instance.HasSpokenWith(npc))
                {
                    return false;
                }
            }
        }

        //Checar Localizações
        if (requiredLocations.Length>0)
        {
            foreach (var location in requiredLocations)
            {
                if (!LocationHistoryTracker.Instance.HasVisited(location))
                {
                    return false;
                }
            }
        }

        //Checar Itens
        /* if (requiredItems.Length>0)
        {
            
        } */
        return true;
    }
}

[System.Serializable]
public class DialogueLine
{
    public ActorSO speaker;
    [TextArea(3, 5)] public string text;
}

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueSO nextDialogue;
}