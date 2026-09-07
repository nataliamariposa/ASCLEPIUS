using UnityEngine;
using System.Collections.Generic; 


public enum RitualOutcome
{
    None,
    Poor,       // offering 1-3, or no confession
    Neutral,    // offering 4-6, basic confession
    Good,       // offering 7-8, sincere confession
    Exceptional // offering 9-10, highly sincere confession
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Night Tracking")]
    public int currentNight = 1;

    [Header("Stats")]
    public int faith = 5;
    public int wifeHealth = 10;
    public int sanity = 10;
    public int asclepiusFavor = 0;

    [Header("Ritual Record")]
    public bool ritualWasPerformed = false;
    public string lastOfferingName = "";
    public int lastOfferingValue = 0;
    public string lastConfessionText = "";
    public int lastConfessionSincerity = 0;
    public RitualOutcome nightOneOutcome = RitualOutcome.None;

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

    public void RecordOffering(ItemData item)
    {
        lastOfferingName = item.itemName;
        lastOfferingValue = item.offeringValue;
        ritualWasPerformed = true;
    }

    public void RecordConfession(ConfessionData confession)
    {
        lastConfessionText = confession.confessionText;
        lastConfessionSincerity = confession.sincerityValue;
    }

    public List<string> collectedItems = new List<string>();

    public void RecordItemCollected(string itemName)
    {
        if (!collectedItems.Contains(itemName))
            collectedItems.Add(itemName);
    }

    public bool WasItemCollected(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    public void EvaluateRitual()
    {
        if (!ritualWasPerformed)
        {
            nightOneOutcome = RitualOutcome.Poor;
            ApplyStatChanges(-2, -1, -1, -3);
            return;
        }

        int combinedScore = lastOfferingValue + lastConfessionSincerity;

        if (combinedScore <= 6)
        {
            nightOneOutcome = RitualOutcome.Poor;
            ApplyStatChanges(1, -1, -1, 1);
        }
        else if (combinedScore <= 11)
        {
            nightOneOutcome = RitualOutcome.Neutral;
            ApplyStatChanges(2, 0, 0, 3);
        }
        else if (combinedScore <= 16)
        {
            nightOneOutcome = RitualOutcome.Good;
            ApplyStatChanges(3, 1, 1, 5);
        }
        else
        {
            nightOneOutcome = RitualOutcome.Exceptional;
            ApplyStatChanges(4, 2, 1, 8);
        }

        Debug.Log($"Ritual evaluated: {nightOneOutcome} (offering: {lastOfferingValue}, confession: {lastConfessionSincerity})");
        currentNight++;
    }

    private void ApplyStatChanges(int f, int wh, int s, int fav)
    {
        faith = Mathf.Clamp(faith + f, 0, 20);
        wifeHealth = Mathf.Clamp(wifeHealth + wh, 0, 20);
        sanity = Mathf.Clamp(sanity + s, 0, 20);
        asclepiusFavor = Mathf.Clamp(asclepiusFavor + fav, 0, 20);
    }

}