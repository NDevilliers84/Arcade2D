using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer _renderer;
    
    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("HurtBox")) {
            _renderer.color = Color.red;
            Invoke(nameof(RollbackColor), 0.25f);
        }
    }
    
    private void RollbackColor() {
        _renderer.color = Color.white;
    }
}