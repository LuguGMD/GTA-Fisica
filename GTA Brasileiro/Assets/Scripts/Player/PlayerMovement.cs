using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PlayerInputs))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedMaxWalk;
    [SerializeField] private float movementSpeedMaxSprint;

    private float currentMaxSpeed;

    private Vector3 direction;
    private Vector2 directionInput;
    private bool isSprinting;

    [HideInInspector] public Rigidbody rb;

    #region Properties

    public float getCurrentMaxSpeed
    {
        get { return currentMaxSpeed; }
        private set { currentMaxSpeed = value; }
    }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentMaxSpeed = movementSpeedMaxWalk;
    }

    private void Update()
    {
        Move();
        Rotate();

        float targetMaxSpeed = this.isSprinting ? movementSpeedMaxSprint : movementSpeedMaxWalk;
        currentMaxSpeed = Mathf.Lerp(currentMaxSpeed, targetMaxSpeed, Time.deltaTime * 3);
    }

    public void Move()
    {
        if (rb.linearVelocity.magnitude < currentMaxSpeed)
        {
            PointTowardsCamera();

            rb.AddForce(direction * movementSpeed);
        }
    }

    public void Rotate()
    {
        Quaternion rotation = transform.rotation;
        transform.LookAt(transform.position + direction);
        transform.rotation = Quaternion.Lerp(rotation, transform.rotation, rb.linearVelocity.magnitude*Time.deltaTime);
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
}
