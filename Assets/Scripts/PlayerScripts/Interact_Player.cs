using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Interact : MonoBehaviour
{
    private float _interactRange = 6f;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private LayerMask _interactLayerMask;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit hit;
            Vector3 origin = _playerCamera.transform.position;
            Vector3 direction = _playerCamera.transform.forward;

            Debug.DrawRay(origin, direction * _interactRange, Color.red, 1f);
            if (Physics.Raycast(origin, direction, out hit, _interactRange, _interactLayerMask))
            {
                IInteractable interactable = hit.collider?.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    // Obtiene la interfaz IInteractor del jugador (este componente o cualquier otro)
                    IInteractor interactor = GetComponent<IInteractor>();
                    interactable.Interact(interactor);
                }
            }
        }
    }
}