using UnityEngine;

public class HingeJointBreak : MonoBehaviour
{
    private void OnJointBreak(float breakForce)
    {
        HingeJoint hingeJoint = GetComponent<HingeJoint>();
        FixedJoint fixedJoint = GetComponent<FixedJoint>();
        
        if (fixedJoint == null || hingeJoint == null)
        {
            transform.parent = null;
        }   
    }
}
