using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;


public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Pause Panel")]
    public GameObject pauseRoot;      // the whole thing, toggled on/off
    public GameObject pausePanel;

    [Header("Player References")]
    public PlayerInteraction playerInteraction;
    public FirstPersonController playerController;
    public StarterAssetsInputs playerInput;

    private bool isPaused = false;
    public bool IsPaused => isPaused;

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
        pauseRoot.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (ScreenFader.Instance != null && ScreenFader.Instance.IsFading) return;

            CutsceneManager cm = FindFirstObjectByType<CutsceneManager>();
            if (cm != null && cm.IsPlaying) return;

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseRoot.SetActive(true);

        if (playerInteraction != null) playerInteraction.enabled = false;
        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.cursorInputForLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (OptionsPanel.Instance != null && OptionsPanel.Instance.IsOpen)
        {
            OptionsPanel.Instance.Close();
            pausePanel.SetActive(true);
            return;
        }

        isPaused = false;
        pauseRoot.SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        // only restore player control if inventory is NOT open
        if (InventoryUI.Instance != null && InventoryUI.Instance.panel.activeSelf)
        {
            // inventory is open, keep cursor unlocked
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (playerInput != null) playerInput.cursorInputForLook = false;
            if (playerController != null) playerController.enabled = false;
        }
        else
        {
            if (playerInteraction != null) playerInteraction.enabled = true;
            if (playerController != null) playerController.enabled = true;
            if (playerInput != null) playerInput.cursorInputForLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    
    public void OpenOptions()
    {
        Debug.Log("OpenOptions called");
        Debug.Log("OptionsPanel.Instance: " + OptionsPanel.Instance);

        if (OptionsPanel.Instance != null)
        {
            Debug.Log("Opening options");
            pausePanel.SetActive(false);
            OptionsPanel.Instance.Open();
        }
    }

    public void QuitToMainMenu()
    {
        isPaused = false;
        ScreenFader.Instance.StartCoroutine(
            ScreenFader.Instance.FadeToScene("MainMenu")
        );
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}