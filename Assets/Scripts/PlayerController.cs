using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    [SerializeField] private float _speed;
    [SerializeField] private float _airControl;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private PolygonCollider2D _hurtBox;
    [SerializeField] private PolygonCollider2D _blockBox;
    [SerializeField] private List<GameplayAction> _comboActions;
    [SerializeField] private List<GameplayAction> _punchActions;
    [SerializeField] private List<GameplayAction> _kickActions;
    [SerializeField] private float _knockbackForce = 8f;
    [SerializeField] private float _hitStunDuration = 0.25f;
    [SerializeField] private float _landDuration = 0.1f;
    [SerializeField] private float _blockDuration = 0.6f;
    [SerializeField] private float _blockCooldown = 1f;
    [SerializeField] private float _blockPushback = 4f;
 
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private State _currentState;
    private InputController _inputController;
    private Health _health;
    private Sprite _lastHurtSprite;
    private Sprite _lastBlockSprite;
    private bool _hurtBoxActive;
    private bool _blockBoxActive;
    private float _nextBlockTime;
    private int _comboIndex;
    private List<GameplayAction> _currentCombo;
    private GameplayAction _currentAction;
    private readonly List<Vector2> _points = new();
 
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float AirControl => _airControl;
    public int Damage => _currentAction != null ? _currentAction.Damage : 0;
    public float KnockbackForce => _knockbackForce;
    public Vector2 KnockbackDirection { get; private set; }
    public bool IsGrounded {
        get {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance, _groundLayer);
            return hit.collider != null;
        }
    }
 
    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputController = GetComponent<InputController>();
        _health = GetComponent<Health>();
 
        _hurtBox.enabled = false;
        _blockBox.enabled = false;
 
        SwitchState(new MoveState(gameObject));
    }
 
    private void Update() {
        _currentState.OnUpdate();
 
        if (_currentState is MoveState && _inputController.IsJumping && IsGrounded) {
            SwitchState(new JumpState(gameObject));
        }
 
        // if (_currentState is JumpState { HasLeftGround: true } && IsGrounded) {
        //     SwitchState(new FallState(gameObject, _landDuration));
        // }
        //
        // if (_currentState is FallState && IsGrounded)
        // {
        //     SwitchState(new LandState(gameObject));
        // }
 
        if (_currentState is LandState { IsFinished: true }) {
            SwitchState(new MoveState(gameObject));
        }
 
        if (_currentState is MoveState && _inputController.IsBlocking.Value && Time.time >= _nextBlockTime) {
            _nextBlockTime = Time.time + _blockDuration + _blockCooldown;
            SwitchState(new BlockState(gameObject, _blockDuration));
        }
 
        if (_currentState is MoveState) {
            if (_inputController.IsAttacking.Value) StartCombo(_comboActions);
            else if (_inputController.IsPunching.Value) StartCombo(_punchActions);
            else if (_inputController.IsKicking.Value) StartCombo(_kickActions);
        }
 
        if (_currentState is AttackState { CanCombo: true } && IsComboButtonPressed()
            && _comboIndex < _currentCombo.Count) {
            StartAttack(_comboIndex);
        }
 
        if (_currentState is AttackState { IsOver: true }) {
            SwitchState(new MoveState(gameObject));
        }
 
        if (_currentState is HitState { IsFinished: true }) {
            SwitchState(new MoveState(gameObject));
        }
 
        if (_currentState is BlockState { IsFinished: true }) {
            SwitchState(new MoveState(gameObject));
        }
    }
 
    private void StartCombo(List<GameplayAction> combo) {
        if (combo == null || combo.Count == 0) return;
 
        _currentCombo = combo;
        StartAttack(0);
    }
 
    private bool IsComboButtonPressed() {
        if (_currentCombo == _comboActions) return _inputController.IsAttacking.Value;
        if (_currentCombo == _punchActions) return _inputController.IsPunching.Value;
        if (_currentCombo == _kickActions) return _inputController.IsKicking.Value;
        return false;
    }
 
    private void StartAttack(int index) {
        _currentAction = _currentCombo[index];
        _comboIndex = index + 1;
        SwitchState(new AttackState(gameObject, _currentAction));
    }
 
    private void SwitchState(State state) {
        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
    }
 
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("HurtBox")) return;
        if (_currentState is DeadState) return;
 
        PlayerController attacker = other.GetComponentInParent<PlayerController>();
        if (attacker == null || attacker == this) return;
        if (!attacker.TryRegisterTarget(this)) return;
 
        TakeHit(attacker, other);
    }
 
    private void TakeHit(PlayerController attacker, Collider2D attackerHurtBox) {
        float side = Mathf.Sign(transform.position.x - attacker.transform.position.x);
 
        if (IsBlocking(attackerHurtBox)) {
            _rigidbody2D.linearVelocity = new Vector2(side * _blockPushback, _rigidbody2D.linearVelocity.y);
            return;
        }
 
        attacker.OnHitConnected();
 
        KnockbackDirection = new Vector2(side, 0.4f).normalized;
 
        bool isDead = _health.TakeDamage(attacker.Damage);
 
        if (isDead) SwitchState(new DeadState(gameObject));
        else SwitchState(new HitState(gameObject, _hitStunDuration));
    }
 
    private bool IsBlocking(Collider2D attackerHurtBox) {
        if (_currentState is not BlockState || !_blockBox.enabled || _blockBox.pathCount == 0) return false;
 
        ColliderDistance2D distance = _blockBox.Distance(attackerHurtBox);
        return distance.isValid && distance.isOverlapped;
    }
 
    public void Kill() {
        if (_currentState is DeadState) return;
 
        _health.TakeDamage(_health.Current);
        SwitchState(new DeadState(gameObject));
    }
 
    private bool TryRegisterTarget(PlayerController target) {
        return _currentState is AttackState attack && attack.TryRegisterTarget(target);
    }
 
    private void OnHitConnected() {
        if (_currentState is AttackState attack) attack.RegisterHit();
    }
 
    private void OnComboWindowOpen() {
        if (_currentState is AttackState attack) attack.OpenComboWindow();
    }
 
    private void OnComboWindowClose() {
        if (_currentState is AttackState attack) attack.CloseComboWindow();
    }
 
    public void OnAttackActive() {
        if (_currentState is not AttackState) return;
 
        _hurtBoxActive = true;
        _lastHurtSprite = null;
        _hurtBox.enabled = true;
    }
 
    public void OnAttackFinished() {
        _hurtBoxActive = false;
        _hurtBox.enabled = false;
    }
 
    public void OnBlockActive() {
        _blockBoxActive = true;
        _lastBlockSprite = null;
        _blockBox.enabled = true;
    }
 
    public void OnBlockFinished() {
        _blockBoxActive = false;
        _blockBox.enabled = false;
    }
 
    private void LateUpdate() {
        if (_hurtBoxActive) CopyShape(_hurtBox, ref _lastHurtSprite);
        if (_blockBoxActive) CopyShape(_blockBox, ref _lastBlockSprite);
    }
 
    private void CopyShape(PolygonCollider2D box, ref Sprite lastSprite) {
        Sprite sprite = _spriteRenderer.sprite;
        if (sprite == lastSprite) return;
        lastSprite = sprite;
 
        int shapeCount = sprite != null ? sprite.GetPhysicsShapeCount() : 0;
        box.pathCount = shapeCount;
 
        for (int i = 0; i < shapeCount; i++) {
            sprite.GetPhysicsShape(i, _points);
 
            if (_spriteRenderer.flipX)
                for (int p = 0; p < _points.Count; p++)
                    _points[p] = new Vector2(-_points[p].x, _points[p].y);
 
            box.SetPath(i, _points);
        }
    }
}