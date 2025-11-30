using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 5.0f;

    [Header("Look Settings")]
    [SerializeField] private float _mouseSensitivity = 5.0f;
    [SerializeField] private float _verticalLookLimit = 80.0f;

    [Header("Component References")]
    [SerializeField] private Transform _cameraTransform;

    private CharacterController _characterController;
    private PlayerInputActions _inputActions;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _xRotation = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        if(_cameraTransform == null)
        {
            Debug.LogError("ERROR: La referencia a la Transform de la camara no esta asignada en el FirstPersonController del jugador.");
            this.enabled = false;
            return;
        }

        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMoveInput;
        _inputActions.Player.Move.canceled += OnMoveInput;
        _inputActions.Player.Look.performed += OnLookInput;
        _inputActions.Player.Look.canceled += OnLookInput;

    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMoveInput;
        _inputActions.Player.Move.canceled -= OnMoveInput;
        _inputActions.Player.Look.performed -= OnLookInput;
        _inputActions.Player.Look.canceled -= OnLookInput;

        _inputActions.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    private void OnLookInput(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.forward * _moveInput.y + transform.right * _moveInput.x;
        _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        float mouseX = _lookInput.x * _mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        float mouseY = _lookInput.y * _mouseSensitivity * Time.deltaTime;
        _xRotation -=mouseY;

        _xRotation = Mathf.Clamp(_xRotation,-_verticalLookLimit,_verticalLookLimit);

        _cameraTransform.localRotation = Quaternion.Euler(_xRotation,0f,0f);
    }

}
