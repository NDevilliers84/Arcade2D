using UnityEngine;

public class JumpState : State {
   
    public bool HasLeftGround { get; private set; }
 
    public JumpState(GameObject host, float duration = -1) : base(host, duration) { }
 
    public override void OnEnter() {
        base.OnEnter();
 
        HasLeftGround = false;
        InputController.JumpPressed.Stop();
 
        Animator.SetBool("IsJumping", true);
        Animator.Play("Jump", 0, 0f);
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(PlayerController.Rigidbody2D.linearVelocity.x, 0f);
        PlayerController.Rigidbody2D.AddForce(Vector2.up * PlayerController.JumpForce, ForceMode2D.Impulse);
    }
 
    public override void OnUpdate() {
        base.OnUpdate();
 
        if (!PlayerController.IsGrounded) HasLeftGround = true;
 
        float velocity = InputController.Move.x * PlayerController.AirControl;
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(velocity, PlayerController.Rigidbody2D.linearVelocity.y);
    }
 
    public override void OnExit() {
        base.OnExit();
 
        Animator.SetBool("IsJumping", false);
    }
}