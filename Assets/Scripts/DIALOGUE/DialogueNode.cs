using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public string[] speakers;

    [TextArea]
    public string[] dialogueLines;

    public DialogueChoice[] choices;
}
