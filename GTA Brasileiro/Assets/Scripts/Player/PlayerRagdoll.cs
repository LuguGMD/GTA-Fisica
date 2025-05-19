using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{

    private bool isRagdollActive = false;
    [SerializeField] private Rigidbody[] ragdollRbs;

    #region Properties

    public bool getIsRagdoolActive
    {
        get { return isRagdollActive; }
        set { isRagdollActive = value; }
    }

    public Rigidbody[] getRagdollRbs
    {
        get { return ragdollRbs; }
        private set {  ragdollRbs = value; }
    }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
