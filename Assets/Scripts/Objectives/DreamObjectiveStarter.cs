using UnityEngine;
using System.Collections;

public class DreamObjectiveStarter : MonoBehaviour
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
        
        ObjectiveManager.Instance.ActivateObjective(
            ObjectiveManager.Instance.objectives[8]
        );
    }
}