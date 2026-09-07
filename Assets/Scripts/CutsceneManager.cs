using UnityEngine;
using UnityEngine.Playables;
using StarterAssets;
using System.Collections;


public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector director;
    public StarterAssetsInputs input;
    public FirstPersonController playerController;
    public bool playOnSceneStart;
    public RitualManager ritual = RitualManager.Instance;
    public Canvas UiCanvas;
    [Header("Post-Cutscene Thought")]
    public bool showThoughtAfterCutscene;
    public ThoughtTrigger endingThought;
    public ThoughtTrigger nightTwoThought;
    public bool IsPlaying { get; private set; }




    private void Start()
    {
        if (playOnSceneStart)
        {
            PlayCutscene();
        }
    }

    public void PlayCutscene()
    {
        IsPlaying = true;
        Debug.Log("Playing Cutscene!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        input.enabled = false;
        playerController.enabled = false;
        UiCanvas.enabled = false;


        director.stopped += EndCutscene;
        director.Play();
    }

    private void EndCutscene(PlayableDirector pd)
    {
        IsPlaying = false;
        Cursor.lockState = CursorLockMode.Locked;
        input.enabled = true;
        playerController.enabled = true;
        UiCanvas.enabled = true;
        director.stopped -= EndCutscene;

        if (showThoughtAfterCutscene)
        {
            if (GameManager.Instance != null && GameManager.Instance.currentNight >= 2)
            {
                if (nightTwoThought != null)
                    nightTwoThought.PlayThought();
            }
            else
            {
                if (endingThought != null)
                    endingThought.PlayThought();
            }
        }

        if (RitualManager.Instance != null &&
            RitualManager.Instance.state == RitualManager.RitualState.Ritual)
        {
            RitualManager.Instance.OnTempleCutsceneFinished();
        }

        if (GameManager.Instance != null && GameManager.Instance.currentNight == 1)
        {
            // activate movement tutorials immediately
            ObjectiveManager.Instance.ActivateObjectives(new ObjectiveData[]
            {
                ObjectiveManager.Instance.objectives[1], // wasd
                ObjectiveManager.Instance.objectives[2]  // mouse
            });

            // activate check on wife after 2 seconds
            StartCoroutine(ActivateWifeObjectiveDelayed());
        }
    }

    private IEnumerator ActivateWifeObjectiveDelayed()
    {
        yield return new WaitForSeconds(4f);
        ObjectiveManager.Instance.ActivateObjective(
            ObjectiveManager.Instance.objectives[0]
        );
    }
}
