using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputSystem_Actions actions;

    private Vector2 moveInput;

    private float doJumpInput;
    private float doSprintInput;
    private float doInteractInput;

    #region Properties

    public Vector2 getMoveInput
    {
        get { return moveInput; }
        private set { moveInput = value; }
    }

    public float getDoJumpInput
    {
        get { return doJumpInput; }
        private set { doJumpInput = value; }
    }

    public float getDoSprintInput
    {
        get { return doSprintInput; }
        private set{ doSprintInput = value; }
    }

    public float getDoInteractInput
    {
        get { return doInteractInput; }
        private set { doInteractInput = value; }
    }

    #endregion

    private void Start()
    {
        ActionsManager.Instance.onPlayerDeath += DisableInputs;
    }

    private void OnEnable()
    {
        actions = new InputSystem_Actions();

        EnableInputs();
    }

    private void OnDisable()
    {
        DisableInputs();
        ActionsManager.Instance.onPlayerDeath -= DisableInputs;
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        ActionsManager.Instance.onPlayerMoveInput?.Invoke(moveInput);
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        doSprintInput = context.ReadValue<float>();
        ActionsManager.Instance.onPlayerSprintInput?.Invoke(doSprintInput);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        doJumpInput = context.ReadValue<float>();
        ActionsManager.Instance.onPlayerJumpInput?.Invoke(doJumpInput);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        doInteractInput = context.ReadValue<float>();
        ActionsManager.Instance.onPlayerInteractInput?.Invoke(doInteractInput);
    }

    public void DisableInputs()
    {
        actions.Player.Move.performed -= Move;
        actions.Player.Move.canceled -= Move;

        actions.Player.Sprint.performed -= Sprint;
        actions.Player.Sprint.canceled -= Sprint;

        actions.Player.Jump.performed -= Jump;
        actions.Player.Jump.canceled -= Jump;

        actions.Player.Interact.performed -= Interact;
        actions.Player.Interact.canceled -= Interact;

        actions.Player.Disable();
    }

    public void EnableInputs()
    {
        actions.Player.Enable();

        actions.Player.Move.performed += Move;
        actions.Player.Move.canceled += Move;

        actions.Player.Sprint.performed += Sprint;
        actions.Player.Sprint.canceled += Sprint;

        actions.Player.Jump.performed += Jump;
        actions.Player.Jump.canceled += Jump;

        actions.Player.Interact.performed += Interact;
        actions.Player.Interact.canceled += Interact;
    }

}
