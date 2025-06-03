using UnityEngine;

public class JointBreak : MonoBehaviour
{
    private void OnJointBreak(float breakForce)
    {
        transform.parent = null;
    }
}
