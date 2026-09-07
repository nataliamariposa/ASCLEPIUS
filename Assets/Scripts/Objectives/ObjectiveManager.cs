using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("Objectives")]
    public ObjectiveData[] objectives;

    private List<int> activeIndices = new List<int>();
    private List<int> completingIndices = new List<int>();
    private HashSet<int> permanentlyCompleted = new HashSet<int>();

    private bool journalCompleted = false;
    private bool keepsakeCompleted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<ObjectiveData> GetActiveObjectives()
    {
        List<ObjectiveData> active = new List<ObjectiveData>();
        List<int> allIndices = new List<int>(activeIndices);
        foreach (int i in completingIndices)
            if (!allIndices.Contains(i))
                allIndices.Add(i);
        foreach (int i in allIndices)
            if (i < objectives.Length && objectives[i] != null)
                active.Add(objectives[i]);
        return active;
    }

    public void ActivateObjective(ObjectiveData objective)
    {
        Debug.Log("ActivateObjective called: " + objective.objectiveText);

        int index = System.Array.IndexOf(objectives, objective);
        Debug.Log("Index found: " + index);
        Debug.Log("Already in activeIndices: " + activeIndices.Contains(index));

        if (index < 0)
        {
            Debug.LogError("Objective not found: " + objective.name);
            return;
        }
        if (activeIndices.Contains(index)) return;
        activeIndices.Add(index);
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.newObjectiveSound);
        RefreshUI();
        StartCoroutine(CheckAlreadyCompletedNextFrame(objective));
    }

    public void ActivateObjectives(ObjectiveData[] objectivesToActivate)
    {
        foreach (var obj in objectivesToActivate)
        {
            int index = System.Array.IndexOf(objectives, obj);
            if (index < 0 || activeIndices.Contains(index)) continue;
            activeIndices.Add(index);
        }
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.newObjectiveSound);
        RefreshUI();
        // check each for already completed
        foreach (var obj in objectivesToActivate)
            StartCoroutine(CheckAlreadyCompletedNextFrame(obj));
    }

    private IEnumerator CheckAlreadyCompletedNextFrame(ObjectiveData objective)
    {
        yield return null;
        yield return null;
        if (IsAlreadyCompleted(objective))
        {
            Debug.Log("Auto completing: " + objective.objectiveText);
            CompleteObjective(objective);
        }
    }

    private bool IsAlreadyCompleted(ObjectiveData objective)
    {
        int index = System.Array.IndexOf(objectives, objective);
        if (permanentlyCompleted.Contains(index)) return true;
        switch (index)
        {
            case 1:
                return FirstPersonController.Instance != null &&
                       FirstPersonController.Instance.hasMovedWASD;
            case 2:
                return FirstPersonController.Instance != null &&
                       FirstPersonController.Instance.hasLooked;
            case 3:
                return InventoryUI.Instance != null &&
                       InventoryUI.Instance.hasOpenedJournal;
            case 4:
                return GameManager.Instance != null &&
                       GameManager.Instance.collectedItems.Count > 0;
            default:
                return false;
        }
    }

    public void CompleteObjective(ObjectiveData objective)
    {
        int index = System.Array.IndexOf(objectives, objective);
        if (index < 0 || !activeIndices.Contains(index)) return;
        if (completingIndices.Contains(index)) return;
        permanentlyCompleted.Add(index);
        StartCoroutine(CompleteRoutine(index));
    }

    private IEnumerator CompleteRoutine(int index)
    {
        completingIndices.Add(index);
        if (ObjectiveUI.Instance != null)
            ObjectiveUI.Instance.ShowCheckmark(objectives[index]);
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.newObjectiveSound);
        yield return new WaitForSecondsRealtime(1.5f);
        activeIndices.Remove(index);
        completingIndices.Remove(index);
        RefreshUI();
    }

    public bool IsCompleting(ObjectiveData objective)
    {
        int index = System.Array.IndexOf(objectives, objective);
        return completingIndices.Contains(index);
    }

    public void NotifyJournalCompleted()
    {
        journalCompleted = true;
        CheckAndActivateTemple();
    }

    public void NotifyKeepsakeCompleted()
    {
        keepsakeCompleted = true;
        CheckAndActivateTemple();
    }

    private void CheckAndActivateTemple()
    {
        if (journalCompleted && keepsakeCompleted)
            ActivateObjective(objectives[5]);
    }

    public void RefreshUI()
    {
        Debug.Log("RefreshUI called, ObjectiveUI.Instance: " + ObjectiveUI.Instance);

        if (ObjectiveUI.Instance != null)
            ObjectiveUI.Instance.RefreshObjectives();
    }
}