using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Keyboard-only movement:
 * Up/Down = move forward/backward
 * Left/Right = rotate player (and camera)
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerKeyboardMover : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float turnSpeed = 180f; // degrees per second

    [SerializeField]
    protected InputAction moveAction;

    private CharacterController controller;

    void OnValidate()
    {
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Value);

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

        // FORWARD / BACKWARD
        Vector3 forwardMove = transform.forward * input.y * moveSpeed * Time.deltaTime;
        controller.Move(forwardMove);

        // ROTATION (turn left/right)
        float turnAmount = input.x * turnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, turnAmount);
    }
}
