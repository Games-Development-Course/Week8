using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Handles player shooting & weapon switching.
 * No movement, no rotation.
 */
public class PlayerShooter : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction fireAction;
    [SerializeField] private InputAction weapon1Action;
    [SerializeField] private InputAction weapon2Action;

    [Header("Weapons")]
    public Weapon gun;
    public Weapon machineGun;

    private Weapon currentWeapon;

    void OnValidate()
    {
        if (fireAction == null)
            fireAction = new InputAction(type: InputActionType.Button);

        if (fireAction.bindings.Count == 0)
            fireAction.AddBinding("<Keyboard>/space");

        if (weapon1Action == null)
            weapon1Action = new InputAction(type: InputActionType.Button);

        if (weapon1Action.bindings.Count == 0)
            weapon1Action.AddBinding("<Keyboard>/1");

        if (weapon2Action == null)
            weapon2Action = new InputAction(type: InputActionType.Button);

        if (weapon2Action.bindings.Count == 0)
            weapon2Action.AddBinding("<Keyboard>/2");
    }

    void OnEnable()
    {
        fireAction.Enable();
        weapon1Action.Enable();
        weapon2Action.Enable();
    }

    void OnDisable()
    {
        fireAction.Disable();
        weapon1Action.Disable();
        weapon2Action.Disable();
    }

    void Start()
    {
        EquipWeapon(gun);
    }

    void Update()
    {
        if (currentWeapon == null) return;

        if (currentWeapon.automatic)
        {
            if (fireAction.IsPressed())
                currentWeapon.TryFire();
        }
        else
        {
            if (fireAction.WasPressedThisFrame())
                currentWeapon.TryFire();
        }

        if (weapon1Action.WasPressedThisFrame())
            EquipWeapon(gun);

        if (weapon2Action.WasPressedThisFrame())
            EquipWeapon(machineGun);
    }

    void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;

        gun.gameObject.SetActive(weapon == gun);
        machineGun.gameObject.SetActive(weapon == machineGun);
    }
}
