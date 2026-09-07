using UnityEngine;
using System.Collections;

public class DreamAltarInteraction : MonoBehaviour
{
    public Light AltarSpotLight;
    private bool hasInteracted = false;

    [Header("Asclepius Dialogue")]
    public DialogueNode poorNeutralDialogue;
    public DialogueNode goodExceptionalDialogue;

    public void Interact()
    {
        if (hasInteracted) return;
        hasInteracted = true;
        ScreenFader.Instance.StartCoroutine(EndDream());
    }

    IEnumerator EndDream()
    {
        Debug.Log("Dream Ending");
        AltarSpotLight.enabled = false;
        ObjectiveManager.Instance.CompleteObjective(
            ObjectiveManager.Instance.objectives[9]
        );
        AudioManager.Instance.StopAmbience();

        yield return new WaitForSeconds(1f);

        // play Asclepius dialogue based on ritual outcome
        DialogueNode nodeToPlay = null;
        if (GameManager.Instance != null)
        {
            RitualOutcome outcome = GameManager.Instance.nightOneOutcome;
            if (outcome == RitualOutcome.Good || outcome == RitualOutcome.Exceptional)
                nodeToPlay = goodExceptionalDialogue;
            else
                nodeToPlay = poorNeutralDialogue;
        }

        if (nodeToPlay != null && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartNode(nodeToPlay);

            // wait for dialogue to finish
            yield return new WaitUntil(() => !DialogueManager.Instance.IsDialogueOpen);
        }

        yield return ScreenFader.Instance.FadeToScene("House");
    }
}