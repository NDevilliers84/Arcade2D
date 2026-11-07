using UnityEngine;

public class MoveState : State {
    public MoveState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnUpdate() {
        base.OnUpdate();

        float x = InputController.Move.x;

        if (x != 0f) PlayerController.SpriteRenderer.flipX = x < 0f;

        Animator.SetFloat("Speed", Mathf.Abs(x));

        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(x * PlayerController.Speed, PlayerController.Rigidbody2D.linearVelocity.y);
    }

    public override void OnExit() {
        base.OnExit();

        Animator.SetFloat("Speed", 0f);

        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);
    }
}