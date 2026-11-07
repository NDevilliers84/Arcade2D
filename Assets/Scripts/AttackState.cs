using UnityEngine;

public class AttackState : State {
    public AttackState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnEnter() {
        base.OnEnter();
        
        InputController.IsAttacking.Stop();
        Animator.speed = 1 / Duration;
        Animator.SetBool("IsAttacking", true);
    }

    public override void OnExit() {
        base.OnExit();

        Animator.speed = 1;
        Animator.SetBool("IsAttacking", false);
    }
    
}