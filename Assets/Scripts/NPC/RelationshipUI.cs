using TMPro;
using UnityEngine;
using System.Collections;

public class RelationshipUI : MonoBehaviour
{
    public static RelationshipUI Instance;

    public GameObject popup;
    public TMP_Text popupText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowRelationshipChange(string message)
    {
        StartCoroutine(ShowPopup(message));
    }

    private IEnumerator ShowPopup(string message)
    {
        popupText.text = message;

        popup.SetActive(true);

        yield return new WaitForSeconds(2f);

        popup.SetActive(false);
    }
}
