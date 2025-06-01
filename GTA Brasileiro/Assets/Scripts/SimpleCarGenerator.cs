using UnityEngine;

/// <summary>
/// Anexe este script a um GameObject vazio (por exemplo, "CarSpawner").
/// Ele cria em Runtime:
///   • Um corpo de carro (Cube + Rigidbody + BoxCollider)
///   • Quatro rodas (cada uma: GameObject com WheelCollider + cilindro visual)
///   • Os componentes CarControl e WheelControl já configurados
/// Tudo para que você não precise ajustar nada na cena.
/// </summary>
public class SimpleCarGenerator : MonoBehaviour
{
    [Header("Parâmetros do Corpo")]
    public Vector3 bodyScale = new Vector3(2f, 1f, 4f);
    public float bodyMass = 1500f;
    public Material bodyMaterial;       // (opcional) material para o corpo

    [Header("Parâmetros das Rodas")]
    public float wheelRadius = 0.5f;
    public float wheelWidth = 0.3f;
    public float suspensionDistance = 0.2f;
    public float suspensionSpring = 35000f;
    public float suspensionDamper = 4500f;
    public float suspensionTargetPosition = 0.5f;

    [Header("Parâmetros de Controle do Carro")]
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float steeringMaxAngle = 30f;
    public float steeringMinAngleAtMaxSpeed = 10f;
    public float centerOfGravityOffset = -1f;

    void Start()
    {
        // 1) Criar o corpo principal (Cube)
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "CarBody";
        body.transform.parent = this.transform;
        body.transform.localPosition = Vector3.zero;
        body.transform.localRotation = Quaternion.identity;
        body.transform.localScale = bodyScale;

        if (bodyMaterial != null)
        {
            var mr = body.GetComponent<MeshRenderer>();
            mr.material = bodyMaterial;
        }

        // Adicionar Rigidbody e ajustar massa e center of mass
        Rigidbody rb = body.AddComponent<Rigidbody>();
        rb.mass = bodyMass;
        // Ajuste temporário do center of mass para maior estabilidade
        Vector3 com = rb.centerOfMass;
        com.y += centerOfGravityOffset;
        rb.centerOfMass = com;

        // (O Cube já vem com BoxCollider por padrão, sem necessidade de MeshCollider)
        // --------------------------------------------------

        // 2) Definir posições relativas das quatro rodas em relação ao centro do corpo.
        //    Aqui, consideramos que o bodyScale.x/2 = metade da largura, bodyScale.z/2 = metade do comprimento.
        float halfWidth = bodyScale.x / 2f;
        float halfLength = bodyScale.z / 2f;
        // Colocamos as rodas "encostando" na lateral e na frente/trás do corpo:
        Vector3[] wheelOffsets = new Vector3[4];
        // Frente esquerda (FL)
        wheelOffsets[0] = new Vector3(-halfWidth, 0f,  halfLength - wheelRadius);
        // Frente direita (FR)
        wheelOffsets[1] = new Vector3( halfWidth, 0f,  halfLength - wheelRadius);
        // Traseira esquerda (RL)
        wheelOffsets[2] = new Vector3(-halfWidth, 0f, -halfLength + wheelRadius);
        // Traseira direita (RR)
        wheelOffsets[3] = new Vector3( halfWidth, 0f, -halfLength + wheelRadius);

        string[] wheelNames = new string[4]
        {
            "Wheel_FL",
            "Wheel_FR",
            "Wheel_RL",
            "Wheel_RR"
        };

        // 3) Criar cada roda: 
        //    • Um GameObject vazio com WheelCollider
        //    • Um cilindro “filho” para visual
        //    • Componente WheelControl configurado (steerable/motorized)
        for (int i = 0; i < 4; i++)
        {
            // 3.1) Criar objeto vazio da roda
            GameObject wheelGO = new GameObject(wheelNames[i] + "_Collider");
            wheelGO.transform.parent = body.transform;

            // A Y do WheelCollider deve ser a altura exata do centro da roda:
            float wheelY = wheelRadius; 
            Vector3 worldOffset = new Vector3(wheelOffsets[i].x, wheelY, wheelOffsets[i].z);
            // Como estamos colocando o carro em (0,0,0), basta localPosition = offset
            wheelGO.transform.localPosition = worldOffset;
            wheelGO.transform.localRotation = Quaternion.identity;

            // 3.2) Adicionar WheelCollider
            WheelCollider wc = wheelGO.AddComponent<WheelCollider>();
            wc.radius = wheelRadius;
            wc.suspensionDistance = suspensionDistance;
            // Configurar spring / damper para suspensão básica
            JointSpring js = wc.suspensionSpring;
            js.spring = suspensionSpring;
            js.damper = suspensionDamper;
            js.targetPosition = suspensionTargetPosition;
            wc.suspensionSpring = js;
            wc.forceAppPointDistance = 0f; // usa padrão, pode ajustar se necessário

            // 3.3) Criar cilindro para visual da roda
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            visual.name = wheelNames[i] + "_Mesh";
            // Remover o collider do cilindro (já usamos WheelCollider separado)
            DestroyImmediate(visual.GetComponent<Collider>());

            // Tornar filho do WheelCollider
            visual.transform.parent = wheelGO.transform;
            visual.transform.localPosition = Vector3.zero;
            // Rotacionar para que a “altura” do cilindro fique ao longo do eixo X
            visual.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            // Ajustar escala para corresponder ao raio e espessura desejados
            visual.transform.localScale = new Vector3(wheelRadius, wheelWidth, wheelRadius);

            // 3.4) Adicionar componente WheelControl e configurar flags
            WheelController wcScript = wheelGO.AddComponent<WheelController>();
            wcScript.wheelModel = visual.transform;
            // Steerable apenas nas rodas da frente (índices 0 e 1)
            wcScript.steerable = (i == 0 || i == 1);
            // Motorized em todas (para simplificar, carro 4WD); se quiser só traseira, coloque (i>=2)
            wcScript.motorized = true;
        }

        // 4) Adicionar CarControl ao corpo e ajustar parâmetros
        CarController carControl = body.AddComponent<CarController>();
        carControl.motorTorque = motorTorque;
        carControl.brakeTorque = brakeTorque;
        carControl.maxSpeed = maxSpeed;
        carControl.steeringRange = steeringMaxAngle;
        carControl.steeringRangeAtMaxSpeed = steeringMinAngleAtMaxSpeed;
        carControl.centreOfGravityOffset = centerOfGravityOffset;
        
        // **Pronto:** ao pressionar Play, o carro surgirá na origem e responderá a WASD ou setas.
    }
}
