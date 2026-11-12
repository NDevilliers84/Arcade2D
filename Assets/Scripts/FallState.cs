using UnityEngine;

public class FallState : State
{
    private const float FallSpeedThreshold = -2f;
 
    private bool _isFalling;
 
    public FallState(GameObject host, float duration = -1) : base(host, duration) { }
 
    public override void OnEnter() {
        base.OnEnter();
 
        _isFalling = false;
        Animator.Play("FlyMid", 0, 0f);
    }
 
    public override void OnUpdate() {
        base.OnUpdate();
 
        float velocity = InputController.Move.x * PlayerController.AirControl;
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(velocity, PlayerController.Rigidbody2D.linearVelocity.y);
 
        if (!_isFalling && PlayerController.Rigidbody2D.linearVelocity.y < FallSpeedThreshold) {
            _isFalling = true;
            Animator.Play("FlyFall", 0, 0f);
        }
    }
    
}