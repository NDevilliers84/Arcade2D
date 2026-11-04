using UnityEngine;

public class MoveState : State {
    public MoveState(GameObject host, float duration = -1) : base(host, duration) { }

    public override void OnUpdate() {
        base.OnUpdate();
        
        PlayerController.transform.Translate(InputController.Move * 
                                              (Time.deltaTime * PlayerController.Speed));
    }
}