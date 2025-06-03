using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PlayerInputs))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController controller;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedMaxWalk;
    [SerializeField] private float movementSpeedMaxSprint;

    private float currentMaxSpeed;

    private Vector3 direction;
    private Vector2 directionInput;
    private bool isSprinting;

    [HideInInspector] public Rigidbody rb;
    private CapsuleCollider col;

    private bool isMovementEnabled = true;

    #region Properties

    public float getCurrentMaxSpeed
    {
        get { return currentMaxSpeed; }
        private set { currentMaxSpeed = value; }
    }

    public bool getIsSprinting
    {
        get { return isSprinting; }
        private set { isSprinting = value; }
    }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<PlayerController>();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        currentMaxSpeed = movementSpeedMaxWalk;

        ActionsManager.Instance.onPlayerEnterCar += DisableMovement;
        ActionsManager.Instance.onPlayerExitCar += EnableMovement;

        ActionsManager.Instance.onPlayerDeath += DisableMovement;

        ActionsManager.Instance.onPlayerRagdollActivate += DisableCollider;
        ActionsManager.Instance.onPlayerRagdollDeactivate += EnableCollider;

        ActionsManager.Instance.onPlayerRagdollActivate += DisableMovement;
        ActionsManager.Instance.onPlayerRagdollDeactivate += EnableMovement;

        ActionsManager.Instance.onPlayerMoveInput += GetDirection;
        ActionsManager.Instance.onPlayerSprintInput += GetSprint;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerEnterCar -= DisableMovement;
        ActionsManager.Instance.onPlayerExitCar -= EnableMovement;

        ActionsManager.Instance.onPlayerDeath -= DisableMovement;

        ActionsManager.Instance.onPlayerRagdollActivate -= DisableCollider;
        ActionsManager.Instance.onPlayerRagdollDeactivate -= EnableCollider;

        ActionsManager.Instance.onPlayerRagdollActivate -= DisableMovement;
        ActionsManager.Instance.onPlayerRagdollDeactivate -= EnableMovement;

        ActionsManager.Instance.onPlayerMoveInput -= GetDirection;
        ActionsManager.Instance.onPlayerSprintInput -= GetSprint;
    }

    private void Update()
    {
        if(isMovementEnabled)
        {
            Move();
            Rotate();

            float targetMaxSpeed = this.isSprinting ? movementSpeedMaxSprint : movementSpeedMaxWalk;
            currentMaxSpeed = Mathf.Lerp(currentMaxSpeed, targetMaxSpeed, Time.deltaTime * 3);
        }
    }

    public void Move()
    {
        if (rb.linearVelocity.magnitude < currentMaxSpeed)
        {
            PointTowardsCamera();

            rb.AddForce(direction * movementSpeed);
        }

        controller.getImpactSpeed = rb.linearVelocity.magnitude;
        controller.getImpactDirection = rb.linearVelocity.normalized;
    }

    public void Rotate()
    {
        Quaternion rotation = transform.rotation;
        transform.LookAt(transform.position + direction);
        transform.rotation = Quaternion.Lerp(rotation, transform.rotation, rb.linearVelocity.magnitude*Time.deltaTime*2);
    }

    public void PointTowardsCamera()
    {
        this.direction = Camera.main.transform.forward * directionInput.y;
        this.direction += Camera.main.transform.right * directionInput.x;
        this.direction.y = 0;

        this.direction = this.direction.normalized;
    }

    public void GetDirection(Vector2 directionInput)
    {
        this.directionInput = directionInput; 
    }

    public void GetSprint(float isSprinting)
    {
        this.isSprinting = isSprinting > 0.1;
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
    }

    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    public void DisableCollider()
    {
        rb.isKinematic = true;
        col.isTrigger = true;
    }

    public void EnableCollider()
    {
        rb.isKinematic = false;
        col.isTrigger = false;
    }
}
