using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private Animator animator;

    private float speed;

    private void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();

        EnableAnimator();

        ActionsManager.Instance.onPlayerRagdollActivate += DisableAnimator;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onPlayerRagdollDeactivate -= DisableAnimator;
    }

    private void Update()
    {
        if (animator != null)
        {
            if(animator.enabled)
            {
                float speed = Mathf.Lerp(animator.GetFloat("Speed"), this.speed, Time.deltaTime*10);
                if (Mathf.Abs(speed - this.speed) <= 0.1) speed = this.speed;
                animator.SetFloat("Speed", speed);
            }
            
        }
    }

    public void ChangeSpeedParameter(float speed)
    {
        this.speed = speed;
    }

    public void DisableAnimator()
    {
        if (animator != null)
            animator.enabled = false;
    }

    public void EnableAnimator()
    {
        if (animator != null)
            animator.enabled = true;
    }
}
