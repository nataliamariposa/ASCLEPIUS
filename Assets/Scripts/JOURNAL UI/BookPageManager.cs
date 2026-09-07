using UnityEngine;

public class BookPageManager : MonoBehaviour
{
    [Header("Pages")]
    public GameObject inventoryPage;
    public GameObject ritualPage;
    public GameObject characterPage;

    private void Start()
    {
        ShowInventory();
    }

    public void ShowInventory()
    {
        inventoryPage.SetActive(true);
        ritualPage.SetActive(false);
        characterPage.SetActive(false);
    }

    public void ShowRitual()
    {
        inventoryPage.SetActive(false);
        ritualPage.SetActive(true);
        characterPage.SetActive(false);
    }

    public void ShowCharacters()
    {
        inventoryPage.SetActive(false);
        ritualPage.SetActive(false);
        characterPage.SetActive(true);
    }
}
