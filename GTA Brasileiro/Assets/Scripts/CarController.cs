using UnityEngine;

/// <summary>
/// Controle de movimento do carro:
/// - Lê Input (“Vertical” e “Horizontal”)
/// - Aplica motorTorque, brakeTorque e steerAngle nos WheelColliders
/// - Ajusta torque/esterceamento conforme velocidade atual (para estabilidade)
/// </summary>
public class CarController : MonoBehaviour
{
    [Header("Parâmetros de Força")]
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float steeringRange = 30f;
    public float steeringRangeAtMaxSpeed = 10f;
    public float centreOfGravityOffset = -1f;

    private WheelController[] wheels;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ajusta centerOfMass novamente (caso queiram refinar)
        Vector3 com = rb.centerOfMass;
        com.y += centreOfGravityOffset;
        rb.centerOfMass = com;

        // Pega todos os WheelController nos filhos
        wheels = GetComponentsInChildren<WheelController>();
    }

    void FixedUpdate()
    {
        // Input padrão (setas ou WASD)
        float vInput = Input.GetAxis("Vertical");   // W/S ou ↑/↓
        float hInput = Input.GetAxis("Horizontal"); // A/D ou ←/→

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
