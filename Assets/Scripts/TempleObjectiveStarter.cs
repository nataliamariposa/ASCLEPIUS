using UnityEngine;
using System.Collections;

public class TempleObjectiveStarter : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        // wait for fade to complete AND ObjectiveUI to exist
        yield return new WaitUntil(() => 
            ScreenFader.Instance != null && !ScreenFader.Instance.IsFading &&
            ObjectiveUI.Instance != null);
        
        yield return null; // one extra frame
        yield return null;

        if (ObjectiveManager.Instance == null) yield break;
        if (GameManager.Instance == null) yield break;
        if (GameManager.Instance.currentNight != 1) yield break;

        ObjectiveManager.Instance.ActivateObjective(
            ObjectiveManager.Instance.objectives[6]
        );
    }
}