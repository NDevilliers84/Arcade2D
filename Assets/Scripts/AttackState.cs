using System.Collections.Generic;
using UnityEngine;

public class AttackState : State {
    
    private readonly GameplayAction _action;
    private readonly HashSet<PlayerController> _targets = new();
    private bool _hasEnteredClip;
 
    public bool HasHit { get; private set; }
    public bool IsComboWindowOpen { get; private set; }
    public bool IsOver { get; private set; }
    public bool CanCombo => HasHit && IsComboWindowOpen;
 
    public AttackState(GameObject host, GameplayAction action) : base(host, -1) {
        _action = action;
    }
 
    public override void OnEnter() {
        base.OnEnter();
 
        InputController.IsAttacking.Stop();
        InputController.IsPunching.Stop();
        InputController.IsKicking.Stop();
 
        PlayerController.Rigidbody2D.linearVelocity =
            new Vector2(0f, PlayerController.Rigidbody2D.linearVelocity.y);
 
        if (!Animator.HasState(0, Animator.StringToHash(_action.AnimationState))) {
            Debug.LogError("L'Animator n'a pas d'état nommé '" + _action.AnimationState + "' (attaque " + _action.name + ")");
            IsOver = true;
            return;
        }
 
        Animator.speed = 1f;
        Animator.SetBool("IsAttacking", true);
        Animator.Play(_action.AnimationState, 0, 0f);
    }
 
    public override void OnUpdate() {
        base.OnUpdate();
 
        AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
 
        if (info.IsName(_action.AnimationState)) {
            _hasEnteredClip = true;
            if (info.normalizedTime >= 1f) IsOver = true;
        }
        else if (_hasEnteredClip) {
            IsOver = true;
        }
    }
 
    public override void OnExit() {
        base.OnExit();
 
        PlayerController.OnAttackFinished();
        IsComboWindowOpen = false;
 
        Animator.SetBool("IsAttacking", false);
    }
 
    public bool TryRegisterTarget(PlayerController target) {
        return _targets.Add(target);
    }
 
    public void RegisterHit() {
        HasHit = true;
    }
 
    public void OpenComboWindow() {
        IsComboWindowOpen = true;
    }
 
    public void CloseComboWindow() {
        IsComboWindowOpen = false;
    }
}