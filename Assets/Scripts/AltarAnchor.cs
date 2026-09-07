using UnityEngine;

public class AltarAnchor : MonoBehaviour
{
    public Transform offerPoint;

    private void Start()
    {
        RitualManager.Instance.SetOfferPoint(offerPoint);
    }
}
