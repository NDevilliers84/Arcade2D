using UnityEngine;

public class BlockState : State
{
    private Color _baseColor;
    public BlockState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerController.OnBlockFinished();
        _baseColor = PlayerController.SpriteRenderer.color;
        PlayerController.SpriteRenderer.color = Color.green;
        Animator.SetBool("IsBlocking", true);
    }

    public override void OnExit()
    {
        base.OnExit();
        PlayerController.SpriteRenderer.color = _baseColor;
        Animator.SetBool("IsBlocking", false);
    }
}

