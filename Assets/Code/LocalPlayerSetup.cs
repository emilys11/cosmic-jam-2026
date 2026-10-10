
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class LocalPlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerInput chickenInput;
    [SerializeField] private PlayerInput eggInput;

    private void Start()
    {
        if (chickenInput == null || eggInput == null)
        {
            Debug.LogError("Assign both PlayerInput components!");
            return;
        }

        Debug.Log($"Gamepads connected: {Gamepad.all.Count}");

        if (Gamepad.all.Count >= 2)
        {
            AssignDevice(chickenInput, Gamepad.all[0]);
            AssignDevice(eggInput, Gamepad.all[1]);
        }
        else if (Keyboard.current != null)
        {
            AssignDevice(chickenInput, Keyboard.current);
            AssignDevice(eggInput, Keyboard.current);
        }

        Debug.Log($"Chicken paired devices: {chickenInput.devices.Count}");
        Debug.Log($"Egg paired devices: {eggInput.devices.Count}");
        Debug.Log($"Chicken map: {chickenInput.currentActionMap?.name}");
        Debug.Log($"Egg map: {eggInput.currentActionMap?.name}");
    }

   private void AssignDevice(PlayerInput player, InputDevice device)
   {
        player.user.UnpairDevices();
        InputUser.PerformPairingWithDevice(device, player.user);

    

        player.onActionTriggered += context =>
        {
            if (context.performed)
            {
                Debug.Log(
                    $"{player.name} received action: " +
                    $"{context.action.name} = " +
                    $"{context.ReadValueAsObject()}"
                );
            }
        };
   }
}
