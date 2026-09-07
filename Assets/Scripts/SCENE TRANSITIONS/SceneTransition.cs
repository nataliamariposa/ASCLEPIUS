using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    public bool askBeforeTransition;

    public void Interact()
    {
        if (ScreenFader.Instance == null)
        {
            Debug.LogError("ScreenFader instance is null!");
            return;
        }

        ScreenFader.Instance.StartCoroutine(
            ScreenFader.Instance.FadeToScene(sceneToLoad)
        );
    }
}