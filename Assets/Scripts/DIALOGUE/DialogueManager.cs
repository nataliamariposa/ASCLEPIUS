using TMPro;
using UnityEngine;
using System.Collections;
using StarterAssets;
using System.Collections.Generic;

public enum DialogueType
{
    NPC,
    Inspectable,
    Thought
}

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public StarterAssetsInputs input;
    public FirstPersonController playerController; 
    public GameObject interactPrompt;
    public static DialogueManager Instance;

    private string[] speakers;
    private string[] lines;

    //CHOICE DIALOGUE SYSTEMS
    private DialogueNode currentNode;
    public GameObject[] choiceButtons;
    public TMP_Text[] choiceTexts;
    private List<int> visibleChoiceIndices = new List<int>();

    private int currentLine;
    public bool IsDialogueOpen => dialoguePanel.activeSelf;
    private DialogueType currentType;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartDialogue(string[] dialogueSpeakers, string[] dialogueLines, DialogueType type)
    {
        speakers = dialogueSpeakers;
        lines = dialogueLines;
        currentLine = 0;

        currentType = type;

        dialoguePanel.SetActive(true);
        if (currentType == DialogueType.NPC)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            input.cursorInputForLook = false;
            playerController.enabled = false;
            interactPrompt.SetActive(false);
        }
        ShowLine();

        if (currentType == DialogueType.Inspectable || currentType == DialogueType.Thought)
        {
            StartCoroutine(AutoClose());
        }
    }

    void ShowLine()
    {
        speakerText.text = speakers[currentLine];
        dialogueText.text = lines[currentLine];
    }

public void AdvanceDialogue()
{
    Debug.Log("AdvanceDialogue called");

    currentLine++;

    Debug.Log("Current line = " + currentLine);
    Debug.Log("Total lines = " + lines.Length);

    if (currentLine >= lines.Length)
    {
        Debug.Log("Reached end of node");

        if (currentNode != null &&
            currentNode.choices != null &&
            currentNode.choices.Length > 0)
        {
            Debug.Log("Showing choices");
            ShowChoices();
        }
        else
        {
            Debug.Log("Closing dialogue");
            CloseDialogue();
        }

        return;
    }

    ShowLine();
}


    //CHOICE DIALOGUE SYSTEM
    public void StartNode(DialogueNode node)
    {
        currentNode = node;
        currentType = DialogueType.NPC;

        speakers = node.speakers;
        lines = node.dialogueLines;

        Debug.Log("Loaded node");
        Debug.Log("Speaker count: " + speakers.Length);
        Debug.Log("Line count: " + lines.Length);


        currentLine = 0;

        dialoguePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        input.cursorInputForLook = false;
        playerController.enabled = false;

        interactPrompt.SetActive(false);

        ShowLine();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
    }

    void ShowChoices()
    {
        visibleChoiceIndices.Clear();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

        int buttonIndex = 0;

        for (int i = 0; i < currentNode.choices.Length; i++)
        {
            DialogueChoice choice = currentNode.choices[i];

            bool canShow = true;

            if (!string.IsNullOrEmpty(choice.requiredFact))
            {
                NPCData npc =
                    NPCDatabase.Instance.GetNPC(choice.factOwner);

            Debug.Log(
                "Checking " +
                choice.factOwner +
                " for fact: " +
                choice.requiredFact
            );
                canShow =
                    npc.discoveredFacts.Contains(choice.requiredFact);
            }

            Debug.Log(
                "Can show: " +
                canShow
            );

            if (canShow)
            {
                choiceButtons[buttonIndex].SetActive(true);

                choiceTexts[buttonIndex].text =
                    choice.choiceText;

                visibleChoiceIndices.Add(i);

                buttonIndex++;
            }
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectChoice(int index)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        DialogueChoice choice =
            currentNode.choices[
                visibleChoiceIndices[index]
            ];

        NPCData npc =
            NPCDatabase.Instance.GetNPC(choice.targetNPC);

        npc.hasMet = true;

        if (npc != null)
        {
            npc.ChangeRelationship(choice.relationshipChange);

            if (choice.relationshipChange > 0)
            {
                RelationshipUI.Instance.ShowRelationshipChange(
                    npc.npcName + " liked your choice."
                );
            }
            else if (choice.relationshipChange < 0)
            {
                RelationshipUI.Instance.ShowRelationshipChange(
                    npc.npcName + " did not like your choice."
                );
            }

            if (!string.IsNullOrEmpty(choice.factToReveal))
            {
                npc.AddFact(choice.factToReveal);
                JournalNotification.Instance.NotifyNewFact();
            }
            Debug.Log("Unlocked fact: " + choice.factToReveal);
            foreach (string fact in npc.discoveredFacts)
            {
                Debug.Log("NPC Fact: " + fact);
            }
        }

        currentNode = choice.nextNode;

        if (currentNode == null)
        {
            CloseDialogue();
            return;
        }

        speakers = currentNode.speakers;
        lines = currentNode.dialogueLines;

        currentLine = 0;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

        ShowLine();
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(3f);
        CloseDialogue();
    }

    private IEnumerator CloseDialogueRoutine()
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

        dialoguePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        input.cursorInputForLook = true;

        input.interact = false;
        input.move = Vector2.zero;
        input.look = Vector2.zero;

        yield return null; // wait 1 frame to avoid input state conflict

        playerController.enabled = true;
    }

    void CloseDialogue()
    {
        StartCoroutine(CloseDialogueRoutine());
    }
}
