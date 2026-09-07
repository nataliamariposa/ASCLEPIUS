using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objectives/Objective")]
public class ObjectiveData : ScriptableObject
{
    public string objectiveText;
    public bool isTutorial;
}