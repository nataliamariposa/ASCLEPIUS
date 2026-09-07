using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RitualManager : MonoBehaviour
{
    public static RitualManager Instance;
    public CutsceneManager cutsceneManager;
    public ConfessionData selectedConfession;
    //offering state bool (within this script)
    public bool offeringPlaced = false;

    public Transform spawnPoint;

    //ALL THE STATES OF A RITUAL
        // -later add Kneeling, etc. 
    public enum RitualState 
    {
        Idle,
        Offering,
        Praying,
        Ritual,
        Sleeping
    } 

    //sets initial state to IDLE
    public RitualState state = RitualState.Idle;    

    //only called once and either initializes the instance or destroys whats there and does it
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

    //THE MAIN FUNCTION TO PLACE THE OFFERING
    public void PlaceOffering(ItemData item)
    {
        Debug.Log("PlaceOffering function working!!!");
        if (offeringPlaced)
        {
            return;
        }
        //creates a 3d object of the same type as what is chosen to go on the altar
        Instantiate(
            item.worldPrefab,
            spawnPoint.position,
            spawnPoint.rotation * Quaternion.Euler(item.spawnRotationOffset)
        );

        //removes from your inventory
        InventoryManager.Instance.RemoveItem(item);

        //toggles the inventory off, since its on from the AltarInteraction script
        InventoryUI.Instance.ToggleInventory();

        //offering state bool (within this script)
        offeringPlaced = true;

        GameManager.Instance.RecordOffering(item);

        ObjectiveManager.Instance.CompleteObjective(
            ObjectiveManager.Instance.objectives[6]
        );
        ObjectiveManager.Instance.ActivateObjective(
            ObjectiveManager.Instance.objectives[7]
        );


        //CHANGES THE STATE TO OFFERING
        state = RitualState.Offering;
        UpdateInventoryContext();

        Debug.Log(item.itemName + " offered.");
        Debug.Log("Current state:" + state);
    }

    //aids with the 3d object spawn
    public void SetOfferPoint(Transform point)
    {
        spawnPoint = point;
    }

    //helper function that helps determine inventory context
    private void UpdateInventoryContext()
    {
        if (state == RitualState.Offering || state == RitualState.Praying || state == RitualState.Ritual)
        {
            InventoryManager.Instance.SetContext(InventoryContext.Altar);
        }
        else
        {
            InventoryManager.Instance.SetContext(InventoryContext.Normal);
        }
    }


    //NEXT TWO ARE TO BEGIN THE RITUAL PROCESS WHICH THE USER HAS NO CONTROL OF, I.E.
    //      -THE STRANGE AND EERY/MAGICAL PART OF THE RITUAL WHICH INCLUDES SOUNDS, MOVEMENTS ETC.
    //      - THE PASSING OUT AT THE END OF IT
    public void BeginRitual()
    {
        state = RitualState.Ritual;
        UpdateInventoryContext();
        Debug.Log(cutsceneManager);
        cutsceneManager = FindFirstObjectByType<CutsceneManager>();
        cutsceneManager.PlayCutscene();
    }

    public void OnTempleCutsceneFinished()
    {
        StartCoroutine(SleepingSequence());
    }

    IEnumerator SleepingSequence()
    {
        Debug.Log("Ritual Begins");
        yield return new WaitForSeconds(2f);
        state = RitualState.Sleeping;
        UpdateInventoryContext();
        GameManager.Instance.EvaluateRitual(); // evaluate before leaving
        ScreenFader.Instance.StartCoroutine(
            ScreenFader.Instance.FadeToScene("Dreams")
        );
    }
}
