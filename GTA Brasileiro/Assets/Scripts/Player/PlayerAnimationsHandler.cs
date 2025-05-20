using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private Animator animator;

    private float speed;

    private void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", speed);
    }

    public void ChangeSpeedParameter(float speed)
    {
        this.speed = speed;
    }
}
