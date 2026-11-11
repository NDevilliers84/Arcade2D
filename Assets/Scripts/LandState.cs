using UnityEngine;

public class LandState : State {
    public LandState(GameObject host, float duration = -1) : base(host, duration) { }
 
    public override void OnEnter() {
        base.OnEnter();
 
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);
 
        Animator.SetBool("IsLanding", true);
        Animator.Play("Land", 0, 0f);
    }
 
    public override void OnExit() {
        base.OnExit();
 
        Animator.SetBool("IsLanding", false);
    }
}
