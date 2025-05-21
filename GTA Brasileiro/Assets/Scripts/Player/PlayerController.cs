using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerRagdoll), typeof(PlayerAnimationsHandler))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerRagdoll playerRagdoll;
    private PlayerInputs playerInputs;
    private PlayerAnimationsHandler playerAnimations;

    #region Properties

    public PlayerMovement getPlayerMovement
    {
        get { return playerMovement; }
        private set { playerMovement = value; }
    }
    public PlayerRagdoll getPlayerRagdoll
    {
        get { return playerRagdoll; }
        private set { playerRagdoll = value; }
    }
    public PlayerInputs getPlayerInput
    {
        get { return playerInputs; }
        private set { playerInputs = value; }
    }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerRagdoll = GetComponent<PlayerRagdoll>();
        playerInputs = GetComponent<PlayerInputs>();
        playerAnimations = GetComponent<PlayerAnimationsHandler>();

        playerInputs.onMove += playerMovement.GetDirection;
        playerInputs.onSprint += playerMovement.GetSprint;
    }

    private void OnDisable()
    {
        playerInputs.onMove -= playerMovement.GetDirection;
        playerInputs.onSprint -= playerMovement.GetSprint;
    }

    private void Update()
    {
        float speed = Mathf.Clamp(playerMovement.rb.linearVelocity.magnitude, 0, playerMovement.getCurrentMaxSpeed);
        playerAnimations.ChangeSpeedParameter(speed);
    }


}
