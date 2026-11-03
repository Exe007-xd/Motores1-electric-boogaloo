using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Interact : MonoBehaviour
{
    private float _interactRange = 7f;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private LayerMask _interactLayerMask;
    private IInteractable _currentInteractable;
    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        RaycastHit hit;

        Vector3 origin = _playerCamera.transform.position;
        Vector3 direction = _playerCamera.transform.forward;
        Debug.DrawRay(
        _playerCamera.transform.position,
        _playerCamera.transform.forward * _interactRange,
        Color.red
        );

        if (Physics.Raycast(origin, direction, out hit, _interactRange, _interactLayerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != _currentInteractable)
            {
                _currentInteractable?.OnLoseFocus();

                _currentInteractable = interactable;

                _currentInteractable?.OnFocus();
            }
        }
        else
        {
            _currentInteractable?.OnLoseFocus();
            _currentInteractable = null;
        }
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            _currentInteractable?.Interact(); 
        }
    }
}