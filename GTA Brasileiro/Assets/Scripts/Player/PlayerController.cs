using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerRagdoll), typeof(PlayerAnimationsHandler))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerRagdoll playerRagdoll;
    private PlayerAnimationsHandler playerAnimations;

    [SerializeField] private LayerMask raycastIgnore;
        [SerializeField]

    private Unity.Cinemachine.CinemachineCamera freeLookCamera;


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
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
        Cursor.visible = false; // Torna o cursor invisível


    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerChangeSpeed -= SetImpact;
        ActionsManager.Instance.onPlayerRagdollActivate -= SendLaunchForceToRagdoll;

    }

    private void OnEnable()
    {
        ActionsManager.Instance.onPlayerChangeSpeed += SetImpact;
        ActionsManager.Instance.onPlayerRagdollActivate += SendLaunchForceToRagdoll;
         freeLookCamera.LookAt = transform; // Ajusta a câmera para olhar para o carro
            freeLookCamera.Follow = transform; // Ajusta a câmera para seguir o carro

    }

    private void Update()
    {
        float speed = 0;
        if(playerMovement.rb.linearVelocity.magnitude > 0.1)
        {
            speed = playerMovement.getIsSprinting ? 1 : 0.5f;
        }
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

    public void ExitCar(Transform position)
    {
        // Reative o mesh renderer do player
        transform.GetChild(0).gameObject.SetActive(true); // Desativa o mesh renderer do player

        Vector3 targetPos = position.position;
        Vector3 startRay = position.transform.parent.position;

        Vector3 direction = (targetPos - startRay).normalized;
        float distance = (targetPos - startRay).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(startRay, direction, out hit, distance,~raycastIgnore))
        {
            targetPos = hit.point - (direction*0.5f);
        }

        transform.position = targetPos; 
            freeLookCamera.LookAt = transform; // Ajusta a câmera para olhar para o carro
            freeLookCamera.Follow = transform; // Ajusta a câmera para seguir o carro

    }

    public void SetImpact(float impactSpeed, Vector3 impactDirection)
    {
        this.impactSpeed = impactSpeed;
        this.impactDirection = impactDirection;
    }

}
