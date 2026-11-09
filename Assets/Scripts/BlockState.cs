using UnityEngine;

public class BlockState : State
{
    private Color _baseColor;
 
    public BlockState(GameObject host, float duration = -1) : base(host, duration) { }
 
    public override void OnEnter() {
        base.OnEnter();
 
        InputController.IsBlocking.Stop();
        PlayerController.OnBlockActive();
 
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);
 
        _baseColor = PlayerController.SpriteRenderer.color;
        PlayerController.SpriteRenderer.color = Color.green;
 
        Animator.SetBool("IsBlocking", true);
    }
 
    public override void OnExit() {
        base.OnExit();
 
        PlayerController.OnBlockFinished();
        PlayerController.SpriteRenderer.color = _baseColor;
 
        Animator.SetBool("IsBlocking", false);
    }
}

