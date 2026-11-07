using UnityEngine;

public class JumpState : State {
    public JumpState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();

        Animator.SetBool("IsJumping", true);
        PlayerController.Rigidbody2D.AddForce(Vector2.up * PlayerController.JumpForce, ForceMode2D.Impulse);
    }

    public override void OnUpdate() {
        base.OnUpdate();

        float velocity = InputController.Move.x * PlayerController.AirControl;
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(velocity, PlayerController.Rigidbody2D.linearVelocity.y);
    }

    public override void OnExit() {
        base.OnExit();

        Animator.SetBool("IsJumping", false);

        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);
    }
}