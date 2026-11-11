using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {

        public Vector2 Move;
        public bool IsJumping;
        public Trigger IsAttacking;
        public Trigger IsPunching;
        public Trigger IsKicking;
        public Trigger IsBlocking;
 
        public void OnMove(InputValue value) {
                Move = value.Get<Vector2>();
        }
 
        public void OnJump(InputValue value) {
                IsJumping = value.isPressed;
        }
 
        public void OnAttack(InputValue value) {
                if (value.isPressed) IsAttacking.Launch();
        }
 
        public void OnPunch(InputValue value) {
                if (value.isPressed) IsPunching.Launch();
        }
 
        public void OnKick(InputValue value) {
                if (value.isPressed) IsKicking.Launch();
        }
 
        public void OnBlock(InputValue value) {
                if (value.isPressed) IsBlocking.Launch();
        }
}
