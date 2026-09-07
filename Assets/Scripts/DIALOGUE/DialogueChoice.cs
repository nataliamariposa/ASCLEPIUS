using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;

    public int relationshipChange;

    public DialogueNode nextNode;

    public NPCType targetNPC;

    public string characterName;

    public string factToReveal;

    public NPCType factOwner;
    
    public string requiredFact;
}