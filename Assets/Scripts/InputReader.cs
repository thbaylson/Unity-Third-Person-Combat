using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking { get; private set; }
    public Vector2 MovementValue { get; private set; }

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action CancelTargetEvent;

    private Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // JumpEvent may be null if there is no one subscribed to it
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // DodgeEvent may be null if there is no one subscribed to it
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    // Cinemachine is handling all Look inputs for us. No need to add code, but we still need the method so that Cinemachine can see it.
    public void OnLook(InputAction.CallbackContext context){}

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        TargetEvent?.Invoke();
    }

    public void OnCancelTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        CancelTargetEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // Boolean allows players to hold the button down to keep attacking.
        // An event would change the behavior to be one attack per button press.
        if (context.performed)
        {
            IsAttacking = true;
        }else if (context.canceled)
        {
            IsAttacking = false;
        }
    }
}
