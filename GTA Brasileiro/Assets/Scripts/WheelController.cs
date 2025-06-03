using UnityEngine;

/// <summary>
/// Para cada roda:
/// - Mantém referência ao cilindro visual (wheelModel)
/// - No Update(), busca a posição e rotação do WheelCollider ➔ aplica ao cilindro
/// </summary>
public class WheelController : MonoBehaviour
{
    [HideInInspector] public WheelCollider WheelCollider;
    public Transform[] wheelModels;    // arraste/associe o cilindro criado
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

        for (int i = 0; i < wheelModels.Length; i++)
        {
            // Aplica no cilindro visual
            if (wheelModels[i] != null)
            {
                wheelModels[i].position = wheelPosition;
                wheelModels[i].rotation = wheelRotation;
            }
        }
        
    }
}
