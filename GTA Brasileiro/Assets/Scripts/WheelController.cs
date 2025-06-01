using UnityEngine;

/// <summary>
/// Para cada roda:
/// - Mantém referência ao cilindro visual (wheelModel)
/// - No Update(), busca a posição e rotação do WheelCollider ➔ aplica ao cilindro
/// </summary>
public class WheelController : MonoBehaviour
{
    [HideInInspector] public WheelCollider WheelCollider;
    public Transform wheelModel;    // arraste/associe o cilindro criado
    public bool steerable = false;  // setado no CarGenerator
    public bool motorized = false;  // setado no CarGenerator

    private Vector3 wheelPosition;
    private Quaternion wheelRotation;

    void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    void Update()
    {
        // Obtém a posição e rotação do WheelCollider no mundo
        WheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);

        // Aplica no cilindro visual
        if (wheelModel != null)
        {
            wheelModel.position = wheelPosition;
            wheelModel.rotation = wheelRotation;
        }
    }
}
