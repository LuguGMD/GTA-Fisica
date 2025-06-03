using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerRagdoll), typeof(PlayerAnimationsHandler))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerRagdoll playerRagdoll;
    private PlayerAnimationsHandler playerAnimations;
        [SerializeField]

    private Unity.Cinemachine.CinemachineCamera freeLookCamera;


    private float impactSpeed;
    private Vector3 impactDirection = new Vector3();

    private float impactSpeed;
    private Vector3 impactDirection = new Vector3();

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

    public float getImpactSpeed
    {
        get { return impactSpeed; }
        set { impactSpeed = value; }
    }
    public Vector3 getImpactDirection
    {
        get { return impactDirection; }
        set { impactDirection = value; }
    }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerRagdoll = GetComponent<PlayerRagdoll>();
        playerAnimations = GetComponent<PlayerAnimationsHandler>();

        freeLookCamera.LookAt = transform; // Ajusta a câmera para olhar para o carro
        freeLookCamera.Follow = transform; // Ajusta a câmera para seguir o carro
        ActionsManager.Instance.onPlayerRagdollActivate += SendLaunchForceToRagdoll;
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
        Cursor.visible = false; // Torna o cursor invisível

        ActionsManager.Instance.onPlayerRagdollActivate += SendLaunchForceToRagdoll;

    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerRagdollActivate -= SendLaunchForceToRagdoll;

    }

    private void OnEnable()
    {
        ActionsManager.Instance.onPlayerRagdollActivate += SendLaunchForceToRagdoll;
         freeLookCamera.LookAt = transform; // Ajusta a câmera para olhar para o carro
            freeLookCamera.Follow = transform; // Ajusta a câmera para seguir o carro
        ActionsManager.Instance.onPlayerRagdollActivate += SendLaunchForceToRagdoll;

    }

    private void Update()
    {
        float speed = Mathf.Clamp(playerMovement.rb.linearVelocity.magnitude, 0, playerMovement.getCurrentMaxSpeed);
        playerAnimations.ChangeSpeedParameter(speed);
    }


    public void SendLaunchForceToRagdoll()
    {
        playerRagdoll.LaunchRagdoll(impactSpeed, impactDirection);
    }

    public void EnterCar()
    {
        // Faça o mesh renderer do player ficar invisível
        playerRagdoll.DeactivateRagdoll(); // Desativa o ragdoll do player
                                           // Desativa o game object do filho
        transform.GetChild(0).gameObject.SetActive(false); // Desativa o mesh renderer do player

    }

    public void ExitCar()
    {
        // Reative o mesh renderer do player
        transform.GetChild(0).gameObject.SetActive(true); // Desativa o mesh renderer do player
        transform.position =  new Vector3(transform.position.x, transform.position.y, transform.position.z - 5); // Ajusta a altura do player ao sair do carro
            freeLookCamera.LookAt = transform; // Ajusta a câmera para olhar para o carro
            freeLookCamera.Follow = transform; // Ajusta a câmera para seguir o carro

    }

}
