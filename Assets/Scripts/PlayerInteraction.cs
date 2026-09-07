using UnityEngine;
using StarterAssets;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float range = 3f;
    public Camera cam;

    private StarterAssetsInputs input;
    public DialogueManager dialogueManager;
    public TMP_Text interactPrompt;
    public CrosshairUI crosshairUI;

    private void Awake()
    {
        input = GetComponent<StarterAssetsInputs>();

        if(input == null)
        {
            Debug.Log("StarterAssetsInputs NOT FOUND on GameObject");
        }
        interactPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleLook();
        UpdateInteractPrompt();
        if (input.interact)
        {
            input.interact = false;

            if (dialogueManager.IsDialogueOpen)
            {
                dialogueManager.AdvanceDialogue();
            }
            else
            {
                TryInteract();
            }
        }
    }

    void TryInteract()
    {
        Debug.Log("INTERACT TRIGGERED");

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            WorldItem item = hit.collider.GetComponentInParent<WorldItem>();
            
            if (item != null)
            {
                item.Interact();
                return;
            }

            SceneTransition transition = hit.collider.GetComponentInParent<SceneTransition>();

            if (transition != null)
            {
               transition.Interact();
                return;
            }

            AltarInteraction altar = hit.collider.GetComponentInParent<AltarInteraction>();

            if (altar != null)
            {
                altar.Interact();
                return;
            }

            TempleBookInteraction book = hit.collider.GetComponentInParent<TempleBookInteraction>();

            if (book != null)
            {
                book.Interact();
                return;
            }

            DialogueTrigger dialogue = hit.collider.GetComponentInParent<DialogueTrigger>();

            if (dialogue != null)
            {
                dialogue.StartConversation();
                return;
            }

            Inspectable inspectable = hit.collider.GetComponentInParent<Inspectable>();
            
            if (inspectable != null)
            {
                inspectable.Inspect();
                return;
            }

            DreamAltarInteraction dreamAltar = hit.collider.GetComponentInParent<DreamAltarInteraction>();

            if (dreamAltar != null)
            {
                dreamAltar.Interact();
                return;
            }

            PrototypeEnding ending = hit.collider.GetComponentInParent<PrototypeEnding>();
            if (ending != null)
            {
                ending.Interact();
                return;
            }
        }
        else 
        {
            crosshairUI.SetDefault();
        }
    }


    void HandleLook()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {

            WorldItem item = hit.collider.GetComponentInParent<WorldItem>();
            if (item != null)
            {
                crosshairUI.SetHand();
                return;
            }
            
            Inspectable inspectable = hit.collider.GetComponentInParent<Inspectable>();
            if (inspectable != null)
            {
                crosshairUI.SetInspect();
                return;
            }

            TempleBookInteraction book = hit.collider.GetComponentInParent<TempleBookInteraction>();

            if (book != null)
            {
                crosshairUI.SetInspect();
                return;
            }

            DialogueTrigger npc = hit.collider.GetComponentInParent<DialogueTrigger>();
            if (npc != null)
            {
                crosshairUI.SetNPC();
                return;
            }

            DreamAltarInteraction dreamAltar = hit.collider.GetComponentInParent<DreamAltarInteraction>();
            if (dreamAltar != null)
            {
                crosshairUI.SetInspect();
            }
        }

        crosshairUI.SetDefault();
    }


    void UpdateInteractPrompt()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            bool canInteract = false;

            if (hit.collider.GetComponentInParent<WorldItem>() != null)
            {
                canInteract = true;
                interactPrompt.text = "[E] Pick Up";
            }
            else if (hit.collider.GetComponentInParent<SceneTransition>() != null)
            {
                canInteract = true;
                interactPrompt.text = "[E] Exit";
            }
            else if (hit.collider.GetComponentInParent<PrototypeEnding>() != null)
            {
                canInteract = true;
                interactPrompt.text = "[E] Exit";
            }
            else if (hit.collider.GetComponentInParent<AltarInteraction>() != null)
            {
                canInteract = true;

                switch (RitualManager.Instance.state)
                {
                    case RitualManager.RitualState.Idle:
                        interactPrompt.text = "[E] Make Offering";
                        break;

                    case RitualManager.RitualState.Offering:
                        interactPrompt.text = "[E] Confess";
                        break;

                    case RitualManager.RitualState.Praying:
                        interactPrompt.text = "[E] Continue Ritual";
                        break;

                    default:
                        interactPrompt.text = "[E] Interact";
                        break;
                }
            }
            else if (hit.collider.GetComponentInParent<TempleBookInteraction>() != null)
            {
                canInteract = true;
                interactPrompt.text = "[E] Read";
            }
            else if (hit.collider.GetComponentInParent<DialogueTrigger>() != null)
            {
                canInteract = true;
                interactPrompt.text = "[E] Speak";
            }
            else if (hit.collider.GetComponentInParent<DreamAltarInteraction>() != null)
            {
                canInteract = true;
                interactPrompt.text = "[E] Examine";
            }
            else if (hit.collider.GetComponentInParent<Inspectable>() != null)
            {
                canInteract = true;
                interactPrompt.text = "[E] Inspect";
            }

            if (dialogueManager.IsDialogueOpen)
            {
                interactPrompt.gameObject.SetActive(false);
                return;
            }

            interactPrompt.gameObject.SetActive(canInteract);
        }
        else
        {
            interactPrompt.gameObject.SetActive(false);
        }
    }
}