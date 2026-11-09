using UnityEngine;

public class AttackState : State {
    private readonly GameplayAction _action;
 
    private float _elapsed;
    private bool _isActive;
 
    public AttackState(GameObject host, GameplayAction action) : base(host, action.Duration) {
        _action = action;
    }
 
    public override void OnEnter() {
        base.OnEnter();
 
        _elapsed = 0f;
        _isActive = false;
 
        InputController.IsAttacking.Stop();
 
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);
 
        Animator.speed = 1 / Duration;
        Animator.SetBool("IsAttacking", true);
        Animator.Play(_action.AnimationState, 0, 0f);
    }
 
    public override void OnUpdate() {
        base.OnUpdate();
 
        _elapsed += Time.deltaTime;
        float progress = _elapsed / Duration;
 
        if (!_isActive && progress >= _action.ActiveStart && progress < _action.ActiveEnd) {
            _isActive = true;
            PlayerController.OnAttackActive();
        }
        else if (_isActive && progress >= _action.ActiveEnd) {
            _isActive = false;
            PlayerController.OnAttackFinished();
        }
    }
 
    public override void OnExit() {
        base.OnExit();
 
        PlayerController.OnAttackFinished();
        PlayerController.OnComboActionFinished();
 
        Animator.speed = 1;
        Animator.SetBool("IsAttacking", false);
    }
    
}