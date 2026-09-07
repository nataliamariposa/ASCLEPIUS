using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance;

    [Header("References")]
    public GameObject panel;
    public Transform objectiveList; // the Vertical Layout Group
    public GameObject objectiveRowPrefab; // the ObjectiveRow prefab

    [Header("Scenes to hide in")]
    public string[] hiddenScenes;

    public List<ObjectiveRow> activeRows = new List<ObjectiveRow>();

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        // pull current state from ObjectiveManager on start
        if (ObjectiveManager.Instance != null)
            RefreshObjectives();

        ObjectiveManager.Instance?.RefreshUI();
    }

    private void OnEnable()
    {
        if (ObjectiveManager.Instance != null)
            RefreshObjectives();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (Instance == this)
            Instance = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool shouldHide = System.Array.Exists(
            hiddenScenes, s => s == scene.name);
        panel.SetActive(!shouldHide);

        if (ObjectiveManager.Instance != null)
            RefreshObjectives();
    }

   public void RefreshObjectives()
    {
        Debug.Log("RefreshObjectives called");

        if (objectiveRowPrefab == null || objectiveList == null || 
            ObjectiveManager.Instance == null)
        {
            Debug.Log("Early return - prefab: " + objectiveRowPrefab + 
                      " list: " + objectiveList + 
                      " manager: " + ObjectiveManager.Instance);
            return;
        }
        List<ObjectiveData> active = ObjectiveManager.Instance.GetActiveObjectives();
        Debug.Log("Active objectives: " + active.Count);

        // remove rows that are no longer active AND not completing
        List<ObjectiveRow> toRemove = new List<ObjectiveRow>();
        foreach (var row in activeRows)
        {
            if (row == null) continue;
            bool stillActive = active.Contains(row.assignedObjective);
            bool isCompleting = ObjectiveManager.Instance.IsCompleting(row.assignedObjective);
            if (!stillActive && !isCompleting)
                toRemove.Add(row);
        }
        foreach (var row in toRemove)
        {
            Destroy(row.gameObject);
            activeRows.Remove(row);
        }

        // add rows for newly activated objectives
        foreach (var objective in active)
        {
            bool alreadyHasRow = false;
            foreach (var row in activeRows)
                if (row != null && row.assignedObjective == objective)
                {
                    alreadyHasRow = true;
                    break;
                }

            if (!alreadyHasRow)
            {
                GameObject rowGO = Instantiate(objectiveRowPrefab, objectiveList);
                ObjectiveRow row = rowGO.GetComponent<ObjectiveRow>();
                row.Setup(objective);
                activeRows.Add(row);
            }
        }

        panel.SetActive(activeRows.Count > 0);
    }


    public void ShowCheckmark(ObjectiveData objective)
    {
        int index = System.Array.IndexOf(
            ObjectiveManager.Instance.objectives, objective);
        
        foreach (var row in activeRows)
        {
            if (row.assignedObjective == objective)
            {
                row.Complete();
                return;
            }
        }
    }
}