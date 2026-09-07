using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("First Dialogue")]
    public DialogueNode firstDialogue;

    [Header("Repeat Dialogue")]
    public DialogueNode repeatDialogue;

    [Header("Objective")]
    public ObjectiveData objectiveToComplete;


    private bool hasTalked = false;

    public void StartConversation()
    {
        if (!hasTalked)
        {
            dialogueManager.StartNode(firstDialogue);
            hasTalked = true;
        }
        else
        {
            dialogueManager.StartNode(repeatDialogue);
        }
        if (objectiveToComplete != null)
        {
            ObjectiveManager.Instance.CompleteObjective(objectiveToComplete);
            // activate next objectives after talking to doctor
            ObjectiveManager.Instance.ActivateObjectives(new ObjectiveData[]
            {
                ObjectiveManager.Instance.objectives[3], // open journal
                ObjectiveManager.Instance.objectives[4]  // find keepsake
            });
        }   
    }
}