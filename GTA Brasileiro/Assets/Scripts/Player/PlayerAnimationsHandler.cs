using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private Animator animator;

    private float speed;

    private void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();

        ActionsManager.Instance.onPlayerRagdollActivate += DisableAnimator;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerRagdollDeactivate -= DisableAnimator;
    }

    private void Update()
    {
        animator.SetFloat("Speed", speed);
    }

    public void ChangeSpeedParameter(float speed)
    {
        this.speed = speed;
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void EnableAnimator()
    {
        animator.enabled = true;
    }
}
