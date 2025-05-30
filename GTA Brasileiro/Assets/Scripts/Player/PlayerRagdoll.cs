using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{

    private bool isRagdollActive = false;
    [SerializeField] private Rigidbody[] ragdollRbs;
    private Collider[] ragdollCols;

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

    private void Start()
    {
        ragdollCols = new Collider[ragdollRbs.Length];
        for (int i = 0; i < ragdollRbs.Length; i++)
        {
            ragdollCols[i] = ragdollRbs[i].GetComponent<Collider>();
        }

        DeactivateRagdoll();

        ActionsManager.Instance.onPlayerDeath += ActivateRagdoll;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerDeath -= ActivateRagdoll;
    }

    [ContextMenu("Activate Ragdoll")]
    public void ActivateRagdoll()
    {
        for (int i = 0; i < ragdollRbs.Length; i++)
        {
            ragdollRbs[i].isKinematic = false;
            ragdollCols[i].isTrigger = false;
        }

        ActionsManager.Instance.onPlayerRagdollActivate?.Invoke();
    }

    public void DeactivateRagdoll()
    {
        for (int i = 0; i < ragdollRbs.Length; i++)
        {
            ragdollRbs[i].isKinematic = true;
            ragdollCols[i].isTrigger = true;
        }

        ActionsManager.Instance.onPlayerRagdollDeactivate?.Invoke();
    }
}
