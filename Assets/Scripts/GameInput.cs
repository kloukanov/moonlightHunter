using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions _playerInputActions;

    public event EventHandler OnSwordAttackAction;
    public event EventHandler OnInteractAction;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.SwordAttack.performed += SwordAttack_performed;
        _playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void SwordAttack_performed(InputAction.CallbackContext obj)
    {
        OnSwordAttackAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() // to clear after scene reloads
    {
        _playerInputActions.Player.SwordAttack.performed -= SwordAttack_performed;

        _playerInputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
