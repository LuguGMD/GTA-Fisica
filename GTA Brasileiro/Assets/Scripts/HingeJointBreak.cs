using UnityEngine;

public class HingeJointBreak : MonoBehaviour
{
    private void OnJointBreak(float breakForce)
    {
        FixedJoint fixedJoint = GetComponent<FixedJoint>();
        
        if (fixedJoint == null)
        {
            transform.parent = null;
        }   
    }
}
