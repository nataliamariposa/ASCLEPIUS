using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class OptionsPanel : MonoBehaviour
{
    public static OptionsPanel Instance;

    [Header("Panel")]
    public GameObject panel;

    [Header("Sensitivity")]
    public Slider sensitivitySlider;

    [Header("Volume")]
    public UnityEngine.UI.Slider sfxSlider;
    public UnityEngine.UI.Slider ambienceSlider;

    [Header("Settings")]
    public float minSensitivity = 0.1f;
    public float maxSensitivity = 3f;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        sensitivitySlider.minValue = minSensitivity;
        sensitivitySlider.maxValue = maxSensitivity;

        float saved = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensitivitySlider.value = saved;

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        ambienceSlider.minValue = 0f;
        ambienceSlider.maxValue = 1f;
        ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVolume", 0.5f);
        ambienceSlider.onValueChanged.AddListener(OnAmbienceVolumeChanged);

        panel.SetActive(false);
    }

    public void Open()
    {
        IsOpen = true;
        float saved = PlayerPrefs.GetFloat("Sensitivity", 1f);
        sensitivitySlider.value = saved;
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVolume", 0.5f);
        panel.SetActive(true);
    }
    
    public void Close()
    {
        IsOpen = false;
        panel.SetActive(false);
        if (PauseManager.Instance != null)
            PauseManager.Instance.pausePanel.SetActive(true);
    }

    private void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();

        FirstPersonController fpc = FindFirstObjectByType<FirstPersonController>();
        if (fpc != null)
            fpc.RotationSpeed = value;
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }

    private void OnAmbienceVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetAmbienceVolume(value);
    }

}