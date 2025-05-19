using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerRagdoll), typeof(PlayerInputs))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerRagdoll playerRagdoll;
    private PlayerInputs playerInputs;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
