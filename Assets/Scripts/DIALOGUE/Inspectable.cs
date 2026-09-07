using UnityEngine;

public class Inspectable : MonoBehaviour
{
    [TextArea]
    public string[] thoughts;

    public DialogueManager dialogueManager;

    public void Inspect()
    {
        string[] emptySpeakers = new string[thoughts.Length];

        for (int i = 0; i < emptySpeakers.Length; i++)
        {
            emptySpeakers[i] = ""; // no name shown
        }

        dialogueManager.StartDialogue(emptySpeakers, thoughts, DialogueType.Inspectable);
    }
}
