using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Stickfight/Gameplay Action")]
public class GameplayAction : ScriptableObject {

    [SerializeField] private string _animationState = "ComboAttack1";
    [SerializeField] private int _damage = 10;

    public string AnimationState => _animationState;
    public int Damage => _damage;
}
