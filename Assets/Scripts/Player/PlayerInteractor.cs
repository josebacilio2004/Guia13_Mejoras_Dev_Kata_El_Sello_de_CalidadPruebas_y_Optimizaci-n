using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float _interactionDistance = 2f;

    private Camera _mainCamera;
    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        _inputActions.Player.Interact.performed -= OnInteract;
        _inputActions.Player.Disable();
    }

    private void Update()
    {
        DetectAndShowFeedBack();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        PerformRayCastInteraction();
    }

    private void PerformRayCastInteraction()
    {
        Ray ray = new Ray(_mainCamera.transform.position,_mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void DetectAndShowFeedBack()
    {
        Ray ray = new Ray(_mainCamera.transform.position,_mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                Debug.Log("Objeto interactuable detectado a la vista del jugador : " + hit.collider.name);
            }
        }
    }

}