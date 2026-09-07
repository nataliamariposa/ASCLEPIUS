using UnityEngine;

//THIS SCRIPT IS THE MAIN COMPONENT ADDED TO THE ALTAR OBJECT IN ORDER FOR
//THE INTERACTION OF THE ALTAR TO HAPPEN. MAIN SCRIPTS THAT RELY ON IT ARE:
//          -InventoryUI
//          -PrayerUI
//          -RitualManager
public class AltarInteraction : MonoBehaviour
{

    public Transform spawnPoint;

    public void Interact()
    {
        RitualManager ritual = RitualManager.Instance;
        InventoryManager.Instance.SetContext(InventoryContext.Altar);

        //checks state if its before placing an object down:
        //Uses InventoryUI
        if (ritual.state == RitualManager.RitualState.Idle)
        {
            InventoryUI.Instance.selectingOffering = true;
            if (!InventoryUI.Instance.panel.activeSelf)
            {
                InventoryUI.Instance.ToggleInventory();
            
            }
        }

        //checks state if its after placing an object down 
        //Uses PrayerUI
        else if(ritual.state == RitualManager.RitualState.Offering)
        {
            PrayerUI.Instance.OpenPrayerUI();
            ritual.state = RitualManager.RitualState.Praying;
        }

    }
}