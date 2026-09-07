using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;

    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TMP_Text interactionText;


    private IInteractable currentInteractable;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        FindInteractable();
    }

    // Called by the Input System UnityEvent
    public void OnInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void FindInteractable()
    {
        currentInteractable = null;

        Collider[] nearbyObjects = Physics.OverlapSphere(
            transform.position,
            interactionRange,
            interactableLayer
        );

        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in nearbyObjects)
        {
            IInteractable interactable =
                collider.GetComponentInParent<IInteractable>();

            if (interactable == null)
                continue;

            if (!IsLookingAt(collider))
                continue;

            float distance = Vector3.Distance(
                transform.position,
                collider.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentInteractable = interactable;
            }
        }

        UpdateInteractionUI();
    }

    private bool IsLookingAt(Collider target)
    {
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionRange,
            interactableLayer
        ))
        {
            IInteractable hitInteractable =
                hit.collider.GetComponentInParent<IInteractable>();

            IInteractable targetInteractable =
                target.GetComponentInParent<IInteractable>();

            return hitInteractable == targetInteractable;
        }

        return false;
    }

    private void UpdateInteractionUI()
    {
        if (currentInteractable == null)
        {
            interactionPrompt.SetActive(false);
            return;
        }

        interactionPrompt.SetActive(true);
        interactionText.text =
            currentInteractable.GetInteractionText();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            interactionRange
        );
    }
}