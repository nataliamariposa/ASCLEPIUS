using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public void PlayClick()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
    }
}
