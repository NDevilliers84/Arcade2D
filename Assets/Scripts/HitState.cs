using UnityEngine;

public class HitState : State {
    private Color _baseColor;

    public HitState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();

        PlayerController.OnAttackFinished();

        _baseColor = PlayerController.SpriteRenderer.color;
        PlayerController.SpriteRenderer.color = Color.red;

        PlayerController.Rigidbody2D.linearVelocity =
            PlayerController.KnockbackDirection * PlayerController.KnockbackForce;

        Animator.SetBool("IsHit", true);
    }

    public override void OnExit() {
        base.OnExit();

        PlayerController.SpriteRenderer.color = _baseColor;

        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);

        Animator.SetBool("IsHit", false);
    }
}
