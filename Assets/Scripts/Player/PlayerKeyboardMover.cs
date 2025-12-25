using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Keyboard-only player movement using InputAction.
 * Arrow keys by default, bindings editable in Inspector.
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerKeyboardMover : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [SerializeField]
    protected InputAction moveAction;

    private CharacterController controller;

    void OnValidate()
    {
        // Create action if missing
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Value);

        // Provide default arrow-key bindings
        if (moveAction.bindings.Count == 0)
        {
            moveAction
                .AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
        }
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, 0f, input.y);

        if (move.sqrMagnitude > 0.001f)
        {
            // Face movement direction
            transform.forward = move.normalized;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
