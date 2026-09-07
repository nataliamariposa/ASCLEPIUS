using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveRow : MonoBehaviour
{
    public Image checkbox;
    public TextMeshProUGUI objectiveText;
    public Sprite uncheckedSprite;
    public Sprite checkedSprite;
    public ObjectiveData assignedObjective;


    public void Setup(ObjectiveData objective)
    {
        assignedObjective = objective;
        objectiveText.text = objective.objectiveText;
        checkbox.sprite = uncheckedSprite;
    }
    public void Complete()
    {
        checkbox.sprite = checkedSprite;
        objectiveText.text = "<s>" + objectiveText.text + "</s>";
    }
}