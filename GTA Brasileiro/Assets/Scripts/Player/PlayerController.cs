using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerRagdoll), typeof(PlayerAnimationsHandler))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerRagdoll playerRagdoll;
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

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerRagdoll = GetComponent<PlayerRagdoll>();
        playerAnimations = GetComponent<PlayerAnimationsHandler>();

        ActionsManager.Instance.onPlayerMoveInput += playerMovement.GetDirection;
        ActionsManager.Instance.onPlayerSprintInput += playerMovement.GetSprint;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerMoveInput -= playerMovement.GetDirection;
        ActionsManager.Instance.onPlayerSprintInput -= playerMovement.GetSprint;
    }

    private void Update()
    {
        float speed = Mathf.Clamp(playerMovement.rb.linearVelocity.magnitude, 0, playerMovement.getCurrentMaxSpeed);
        playerAnimations.ChangeSpeedParameter(speed);
    }


}
