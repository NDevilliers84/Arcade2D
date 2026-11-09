using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private Health[] _players;
    [SerializeField] private float _restartDelay = 3f;

    private bool _isOver;

    private void OnEnable() {
        foreach (Health player in _players) player.Died += OnPlayerDied;
    }

    private void OnDisable() {
        foreach (Health player in _players) player.Died -= OnPlayerDied;
    }

    private void OnPlayerDied(Health loser) {
        if (_isOver) return;
        _isOver = true;
        Invoke(nameof(Restart), _restartDelay);
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
