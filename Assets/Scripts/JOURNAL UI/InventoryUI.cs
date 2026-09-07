using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject panel;
    public Image[] icons;
    public StarterAssetsInputs input;
    public FirstPersonController playerController;
    public PlayerInteraction playerInteraction;
    public bool hasOpenedJournal = false;

    // Item preview
    public Image previewImage;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    // IMPORTANT (used for altar system)
    public bool selectingOffering = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        panel.SetActive(false);
        RefreshUI();
    }

    private void Update()
    {
        if (input == null) return;
        if (input.inventory)
        {
            input.inventory = false;
            Debug.Log("Inventory key pressed");
            
            if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            {
                Debug.Log("Blocked by pause");
                return;
            }
            if (PrayerUI.Instance != null && PrayerUI.Instance.panel.activeSelf)
            {
                Debug.Log("Blocked by prayer UI");
                return;
            }
            Debug.Log("Current scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Dreams") return;
            
            ToggleInventory();
        }
    }

    // -------------------------
    // INVENTORY REFRESH
    // -------------------------
    public void RefreshUI()
    {
        var items = InventoryManager.Instance.items;

        for (int i = 0; i < icons.Length; i++)
        {
            if (i < items.Count)
            {
                icons[i].sprite = items[i].icon;
                icons[i].enabled = true;
            }
            else
            {
                icons[i].sprite = null;
                icons[i].enabled = false;
            }
        }
    }

    // -------------------------
    // OPEN / CLOSE INVENTORY
    // -------------------------
    public void ToggleInventory()
    {
        if (DialogueManager.Instance.IsDialogueOpen)
            return;

        bool isOpen = panel.activeSelf;
        panel.SetActive(!isOpen);

        if (!isOpen)
        {
            if (!hasOpenedJournal)
            {
                hasOpenedJournal = true;
                ObjectiveManager.Instance.CompleteObjective(
                    ObjectiveManager.Instance.objectives[3]
                );
                ObjectiveManager.Instance.NotifyJournalCompleted();
            }

            Debug.Log("Inventory opened");
            AudioManager.Instance.PlayJournalOpen();
            JournalNotification.Instance.ClearUnread();
            JournalNotification.Instance.journalIcon.gameObject.SetActive(false); // hide
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            input.cursorInputForLook = false;
            playerController.enabled = false;
        }
        else
        {
            Debug.Log("Inventory closed");
            AudioManager.Instance.PlayJournalClose();
            JournalNotification.Instance.journalIcon.gameObject.SetActive(true); // show

            if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            {
                // game is paused, keep cursor unlocked but don't restore camera
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                input.cursorInputForLook = false;
                playerController.enabled = false;
            }
            else
            {
                panel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                input.cursorInputForLook = true;
                playerController.enabled = true;
                Time.timeScale = 1f; // add this
                if (playerInteraction != null) playerInteraction.enabled = true;
            }
        }
    }

    // -------------------------
    // ITEM SELECTION
    // -------------------------
    public void SelectItem(int index)
    {
        var items = InventoryManager.Instance.items;

        if (index >= items.Count)
            return;

        ItemData item = items[index];

        previewImage.sprite = item.icon;
        previewImage.enabled = true;

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
    }

    // -------------------------
    // OPEN INVENTORY DIRECTLY
    // -------------------------
    public void OpenInventory()
    {
        panel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        input.cursorInputForLook = false;
        playerController.enabled = false;
    }
}