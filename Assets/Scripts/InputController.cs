using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {

        public Vector2 Move;
        public bool IsJumping;
        public Trigger IsAttacking;

        public void OnMove(InputValue value) {
                Move = value.Get<Vector2>();
        }

        public void OnJump(InputValue value) {
                IsJumping = value.isPressed;
        }

        public void OnAttack(InputValue value) {
                if (value.isPressed) IsAttacking.Launch();
        }
}
