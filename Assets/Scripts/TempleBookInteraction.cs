using UnityEngine;

public class TempleBookInteraction : MonoBehaviour
{
    private bool hasBeenRead = false;

    public BookPageManager bookUI; // assign in Inspector

    public void Interact()
    {
        if (!hasBeenRead)
        {
            hasBeenRead = true;
        }

        bookUI.ShowRitual();
    }
}
