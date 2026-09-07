using UnityEngine;

public class ThoughtTrigger : MonoBehaviour
{
    [TextArea]
    public string thought;
    public string speaker;

    public void PlayThought()
    {
        DialogueManager.Instance.StartDialogue(
            new string[] { speaker },
            new string[] { thought },
            DialogueType.Thought
            );
    }
}
