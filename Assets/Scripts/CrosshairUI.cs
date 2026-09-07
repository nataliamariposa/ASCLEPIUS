using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public Image crosshair;

    public Sprite defaultSprite;
    public Sprite inspectSprite;
    public Sprite handSprite;
    public Sprite npcSprite;

    public void SetDefault()
    {
        crosshair.sprite = defaultSprite;
    }

    public void SetInspect()
    {
        crosshair.sprite = inspectSprite;
    }

    public void SetHand()
    {
        crosshair.sprite = handSprite;
    }

    public void SetNPC()
    {
        crosshair.sprite = npcSprite;
    }
}