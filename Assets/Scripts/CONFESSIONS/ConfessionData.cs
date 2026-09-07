using UnityEngine;

[CreateAssetMenu(menuName = "Ritual/Confession")]
public class ConfessionData : ScriptableObject
{
    [TextArea]
    public string confessionText;

    public NPCType factOwner;

    public string requiredFact;

    [TextArea]
    public string learnedFrom;

    public int sincerityValue = 5;
}
