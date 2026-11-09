using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Stickfight/Gameplay Action")]
public class GameplayAction : ScriptableObject {
    
    [SerializeField] private string _animationState = "ComboAttack1";
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _recovery = 0.15f;
    [SerializeField, Range(0f, 1f)] private float _activeStart = 0.3f;
    [SerializeField, Range(0f, 1f)] private float _activeEnd = 0.7f;
 
    public string AnimationState => _animationState;
    public float Duration => _duration;
    public int Damage => _damage;
    public float Recovery => _recovery;
    public float ActiveStart => _activeStart;
    public float ActiveEnd => _activeEnd;
}
