using UnityEngine;

public class NightTwoResponder : MonoBehaviour
{
    [Header("Doctor")]
    public DialogueTrigger doctorDialogueTrigger;
    public DialogueNode doctorNightTwoPoor;
    public DialogueNode doctorNightTwoNeutral;
    public DialogueNode doctorNightTwoGood;
    public DialogueNode doctorNightTwoExceptional;
    public DialogueNode doctorNightTwoRepeat;

    [Header("Atmosphere Objects")]
    public GameObject[] nightOneObjects;
    public GameObject[] nightTwoObjects;

    [Header("Door")]
    public SceneTransition doorSceneTransition;

    private void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentNight < 2) return;

        ApplyNightTwoChanges();
    }

    private void ApplyNightTwoChanges()
    {
        SwapAtmosphere();
        SwapDoctorDialogue();
        SwapDoor();
    }

    private void SwapAtmosphere()
    {
        foreach (var obj in nightOneObjects)
            if (obj != null) obj.SetActive(false);

        foreach (var obj in nightTwoObjects)
            if (obj != null) obj.SetActive(true);
    }

    private void SwapDoor()
    {
        if (doorSceneTransition == null) return;
        if (GameManager.Instance.currentNight >= 2)
            doorSceneTransition.enabled = false;
    }

    private void SwapDoctorDialogue()
    {
        if (doctorDialogueTrigger == null) return;

        DialogueNode node = GameManager.Instance.nightOneOutcome switch
        {
            RitualOutcome.Poor        => doctorNightTwoPoor,
            RitualOutcome.Neutral     => doctorNightTwoNeutral,
            RitualOutcome.Good        => doctorNightTwoGood,
            RitualOutcome.Exceptional => doctorNightTwoExceptional,
            _                         => null
        };

        if (node != null)
            doctorDialogueTrigger.firstDialogue = node;

        if (doctorNightTwoRepeat != null)
            doctorDialogueTrigger.repeatDialogue = doctorNightTwoRepeat; 
    }
}