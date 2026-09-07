using UnityEngine;

public class SceneAmbience : MonoBehaviour
{
    public enum AmbienceType { None, Night, Temple, MainMenu, Dream }
    public AmbienceType ambience;

    private void Start()
    {
        if (AudioManager.Instance == null) return;
        switch (ambience)
        {
            case AmbienceType.Night:
                AudioManager.Instance.PlayAmbience(
                    AudioManager.Instance.nightAmbience, true);
                break;
            case AmbienceType.Temple:
                AudioManager.Instance.PlayAmbience(
                    AudioManager.Instance.templeAtmosphere, true);
                break;
            case AmbienceType.MainMenu:
                AudioManager.Instance.PlayAmbience(
                    AudioManager.Instance.mainTheme, true);
                break;
            case AmbienceType.Dream:
                AudioManager.Instance.PlayAmbience(
                    AudioManager.Instance.dreamAmbience, true);
                break;
            case AmbienceType.None:
                AudioManager.Instance.StopAmbience();
                break;
        }
    }
}