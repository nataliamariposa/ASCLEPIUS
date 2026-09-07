using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterJournalUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;          // ScrollView Content
    public GameObject npcButtonPrefab;       // Button prefab

    [Header("Detail Panel")]
    public TMP_Text nameText;
    public TMP_Text relationshipText;
    public TMP_Text summaryText;
    public TMP_Text factsText;

    private NPCData selectedNPC;

    private void OnEnable()
    {
        RefreshNPCList();
        ClearDetails();
    }

    // -----------------------------
    // BUILD LEFT SIDE LIST
    // -----------------------------
    public void RefreshNPCList()
    {
        // Clear old buttons
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Get all NPCs from database
        foreach (NPCData npc in NPCDatabase.Instance.GetAllNPCs())
        {
            // Only show NPCs player has met
            if (!npc.hasMet)
                continue;

            GameObject buttonObj =
                Instantiate(npcButtonPrefab, contentParent);

            TMP_Text buttonText =
                buttonObj.GetComponentInChildren<TMP_Text>();

            buttonText.text = npc.npcName;

            Button button =
                buttonObj.GetComponent<Button>();

            // IMPORTANT: capture local variable
            NPCData capturedNPC = npc;

            button.onClick.AddListener(() =>
            {
                ShowNPCDetails(capturedNPC);
            });
        }
    }

    // -----------------------------
    // RIGHT SIDE DETAILS
    // -----------------------------
    public void ShowNPCDetails(NPCData npc)
    {
        selectedNPC = npc;
        RefreshDetails();
    }

    public void RefreshDetails()
    {
        if (selectedNPC == null)
            return;

        nameText.text = selectedNPC.npcName;

        relationshipText.text =
            selectedNPC.GetRelationshipTitle() +
            " (" + selectedNPC.relationship + ")";

        summaryText.text = selectedNPC.summary;

        // Build facts list
        factsText.text = "";

        if (selectedNPC.discoveredFacts.Count == 0)
        {
            factsText.text = "No information discovered yet.";
            return;
        }

        foreach (string fact in selectedNPC.discoveredFacts)
        {
            factsText.text += "• " + fact + "\n";
        }
    }

    public void ClearDetails()
    {
        nameText.text = "";
        relationshipText.text = "";
        summaryText.text = "";
        factsText.text = "";
    }
}
