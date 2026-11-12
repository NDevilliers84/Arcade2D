using UnityEngine;

public class FallState : State
{
    public FallState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter()
    {
        base.OnEnter();

        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);

        Animator.SetFloat("Fall", PlayerController.Rigidbody2D.linearVelocity.magnitude);
        Animator.Play("Fall");
    }

    public override void OnExit()
    {
        base.OnExit();
        
        Animator.SetFloat("Fall", PlayerController.Rigidbody2D.linearVelocity.magnitude);
    }
    
}