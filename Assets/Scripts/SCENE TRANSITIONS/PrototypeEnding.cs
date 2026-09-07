using System.Collections;
using UnityEngine;
using TMPro;
using StarterAssets;


public class PrototypeEnding : MonoBehaviour
{
    [SerializeField] private Canvas endingCanvas;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private TextMeshProUGUI toBeContText;
    public StarterAssetsInputs input;
    public FirstPersonController playerController;


    public void Interact()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentNight >= 2)
        {
            ScreenFader.Instance.StartCoroutine(TriggerEnding());
        }
        else
        {
            // night one, transition normally to temple
            ObjectiveManager.Instance.CompleteObjective(ObjectiveManager.Instance.objectives[5]);

            ScreenFader.Instance.StartCoroutine(ScreenFader.Instance.FadeToScene("Temple"));
        }
    }

    private IEnumerator TriggerEnding()
    {
        yield return ScreenFader.Instance.FadeOut();
        endingCanvas.gameObject.SetActive(true);
        SetEndingText();
        toBeContText.text = "To Be Continued...";
        Cursor.visible = false;
        input.enabled = false;
        playerController.enabled = false;

        yield return ScreenFader.Instance.FadeIn();

        yield return new WaitForSeconds(5f); // adjust to how long you want

        yield return ScreenFader.Instance.FadeToScene("MainMenu");
    }

    private void SetEndingText()
    {
        switch (GameManager.Instance.nightOneOutcome)
        {
            case RitualOutcome.Poor:
                endingText.text = "You leave the house again.\nThe air feels heavier than yesterday.";
                break;
            case RitualOutcome.Neutral:
                endingText.text = "You leave the house again.\nSomething has changed. You're not sure what.";
                break;
            case RitualOutcome.Good:
                endingText.text = "You leave the house again.\nFor the first time, you feel like there might be a way through this.";
                break;
            case RitualOutcome.Exceptional:
                endingText.text = "You leave the house again.\nAsclepius heard you. Now you must find out why he had to.";
                break;
        }
    }
}