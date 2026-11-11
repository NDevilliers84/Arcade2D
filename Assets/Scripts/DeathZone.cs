using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DeathZone : MonoBehaviour {

    private void Reset() {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null) player.Kill();
    }
}
