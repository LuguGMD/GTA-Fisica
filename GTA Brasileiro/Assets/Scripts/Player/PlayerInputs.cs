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


    public Action<Vector2> onMove;
    public Action<float> onSprint;
    public Action<float> onJump;
    public Action<float> onInteract;
    

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
        actions.Player.Move.performed += Move;
        actions.Player.Move.canceled += Move;

        actions.Player.Sprint.performed += Sprint;
        actions.Player.Sprint.canceled += Sprint;

        actions.Player.Jump.performed += Jump;
        actions.Player.Jump.canceled += Jump;

        actions.Player.Interact.performed += Interact;
        actions.Player.Interact.canceled += Interact;
    }

    private void OnEnable()
    {
        actions = new InputSystem_Actions();

        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        onMove?.Invoke(moveInput);
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        doSprintInput = context.ReadValue<float>();
        onSprint?.Invoke(doSprintInput);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        doJumpInput = context.ReadValue<float>();
        onJump?.Invoke(doJumpInput);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        doInteractInput = context.ReadValue<float>();
        onInteract?.Invoke(doInteractInput);
    }

}
