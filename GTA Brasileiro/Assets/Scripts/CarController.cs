using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Controle de movimento do carro:
/// - Lê Input (“Vertical” e “Horizontal”)
/// - Aplica motorTorque, brakeTorque e steerAngle nos WheelColliders
/// - Ajusta torque/esterceamento conforme velocidade atual (para estabilidade)
/// </summary>
public class CarController : MonoBehaviour
{
    public Transform playerExitCarPoint;

    [Header("Parâmetros de batida")]
    public float crashSpeed = 10f;
    public Transform playerCrashExitPoint;

    [Header("Parâmetros de Força")]
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float steeringRange = 30f;
    public float steeringRangeAtMaxSpeed = 10f;
    public float centreOfGravityOffset = -1f;
    public bool isPlayerControlled = true;
    private WheelController[] wheels;
    private Rigidbody rb;
    private PlayerController playerController;
    [SerializeField]
    private Unity.Cinemachine.CinemachineCamera freeLookCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = FindAnyObjectByType<PlayerController>();

        // Ajusta centerOfMass novamente (caso queiram refinar)
        Vector3 com = rb.centerOfMass;
        com.y += centreOfGravityOffset;
        rb.centerOfMass = com;

        // Pega todos os WheelController nos filhos
        wheels = GetComponentsInChildren<WheelController>();
    }

    void Update()
    {

        // Se o jogador apertar E e sua distância do carro for menor que 3, entra no carro
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            if (Vector3.Distance(playerController.transform.position, transform.position) < 2f)
            {
                playerController.EnterCar();
                isPlayerControlled = false;
                ActionsManager.Instance.onPlayerEnterCar?.Invoke();

                freeLookCamera.LookAt = transform;
                freeLookCamera.Follow = transform;
                return;
            }
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {

            // Desparenteia o player e o coloca fora do carro

            if (isPlayerControlled)
            {
                return;
            }

            playerController.ExitCar(playerExitCarPoint);
            ActionsManager.Instance.onPlayerExitCar?.Invoke();
            isPlayerControlled = true;
            return;
        }
    }

    void FixedUpdate()
    {
        float vInput = 0; // W/S ou ↑/↓
        float hInput = 0; // A/D ou ←/→
                        // Input padrão (setas ou WASD)

        if (!isPlayerControlled)
        {
            vInput = Input.GetAxis("Vertical");
            hInput = Input.GetAxis("Horizontal");
        }
        // If presses E, exit car

        

        // Calcula velocidade atual na direção dianteira do carro
        float forwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(forwardSpeed));

        // Reduz torque e ângulo de esterço em altas velocidades
        float currentMotor = Mathf.Lerp(motorTorque, 0f, speedFactor);
        float currentSteer = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Verifica se está acelerando no mesmo sentido da velocidade
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            if (wheel != null)
            {
                WheelCollider wc = wheel.WheelCollider;

                // Se for steerable, aplica steerAngle
                if (wheel.steerable)
                {
                    wc.steerAngle = hInput * currentSteer;
                }

                if (isAccelerating)
                {
                    // Se for motorized, aplica torque; solta freio
                    if (wheel.motorized)
                    {
                        wc.motorTorque = vInput * currentMotor;
                    }
                    wc.brakeTorque = 0f;
                }
                else
                {
                    // Se mudando de sentido, zera motorTorque e aplica freio
                    wc.motorTorque = 0f;
                    wc.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica colisão com o jogador
        if (collision.gameObject.CompareTag("Player"))
        {
            ActionsManager.Instance.onPlayerEnterCar?.Invoke();
            // collision.transform.SetParent(transform);
            isPlayerControlled = false;
            playerController.EnterCar();
            freeLookCamera.LookAt = transform; // Ajusta a câmera para olhar para o carro
            freeLookCamera.Follow = transform; // Ajusta a câmera para seguir o carro
        }
        else
        {
            float impactSpeed = collision.relativeVelocity.magnitude;
            if (impactSpeed >= crashSpeed && !isPlayerControlled)
            {
                

                isPlayerControlled = true;
                playerController.ExitCar(playerCrashExitPoint);
                ActionsManager.Instance.onPlayerExitCar?.Invoke();
                ActionsManager.Instance.onPlayerChangeSpeed(impactSpeed, -collision.relativeVelocity.normalized);
                ActionsManager.Instance.onPlayerDeath?.Invoke();
            }
            
        }

    }
}