using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;

public class PrayerUI : MonoBehaviour
{
    public static PrayerUI Instance;
    public GameObject panel;
    public StarterAssetsInputs input;
    public FirstPersonController playerController;
    public Canvas UICanvas;

    
    //FOR CONFESSIONS
    public ConfessionData[] allConfessions;
    public GameObject[] confessionButtons;
    public TMP_Text[] confessionTexts;

    private ConfessionData[] displayedConfessions = new ConfessionData[4];


    private void Start()
    {
        panel.SetActive(false);
    }

    private void Awake()
    {
        if (Instance == null)
        {
        Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenPrayerUI()
    {
        BuildConfessions();
        //Time.timeScale = 0f;
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        input.cursorInputForLook = false;
        playerController.enabled = false;
        UICanvas.enabled = false;



    }

    public void ClosePrayerUI()
    {
        panel.SetActive(false);
        //Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        input.cursorInputForLook = true;
        playerController.enabled = true;
        UICanvas.enabled = true;

    }

    public void SayPrayer()
    {
        Debug.Log("Prayer Spoken");
        // if no confession was selected, record empty sincerity
        if (RitualManager.Instance.selectedConfession == null)
        {
            GameManager.Instance.lastConfessionSincerity = 0;
        }
        ClosePrayerUI();
        RitualManager.Instance.BeginRitual();
    }

    void BuildConfessions()
    {
        for (int i = 0; i < confessionButtons.Length; i++)
        {
            confessionButtons[i].SetActive(false);
        }

        int buttonIndex = 0;

        foreach (ConfessionData confession in allConfessions)
        {
            bool unlocked = true;

            if (!string.IsNullOrEmpty(confession.requiredFact))
            {
                NPCData npc =
                    NPCDatabase.Instance.GetNPC(
                        confession.factOwner);

                unlocked =
                    npc.discoveredFacts.Contains(
                        confession.requiredFact);
            }

            if (unlocked)
            {
                confessionButtons[buttonIndex].SetActive(true);

                confessionTexts[buttonIndex].text =
                    confession.confessionText;

                displayedConfessions[buttonIndex] =
                    confession;

                buttonIndex++;
            }
        }
    }

    public void SelectConfession(int index)
    {
        RitualManager.Instance.selectedConfession =
            displayedConfessions[index];

        GameManager.Instance.RecordConfession(displayedConfessions[index]);
        
        ObjectiveManager.Instance.CompleteObjective(
            ObjectiveManager.Instance.objectives[7]
        );


        Debug.Log(
            "Selected confession: " +
            displayedConfessions[index].confessionText
        );

        ClosePrayerUI();

        RitualManager.Instance.BeginRitual();
    }
}
