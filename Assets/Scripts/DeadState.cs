using UnityEngine;

public class DeadState : State {
    public DeadState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();

        PlayerController.OnAttackFinished();
        PlayerController.OnBlockFinished();
        PlayerController.Rigidbody2D.linearVelocity = Vector2.zero;

        Animator.SetBool("IsDead", true);
    }
}
