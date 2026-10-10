using UnityEngine;
using UnityEngine.InputSystem;

public class AssignGamepads : MonoBehaviour
{
    public PlayerInput chicken;
    public PlayerInput egg;

    void Start()
    {
        var pads = Gamepad.all;

        if (pads.Count > 0)
            chicken.SwitchCurrentControlScheme("Gamepad", pads[0]);

        if (pads.Count > 1)
            egg.SwitchCurrentControlScheme("Gamepad", pads[1]);
    }
}