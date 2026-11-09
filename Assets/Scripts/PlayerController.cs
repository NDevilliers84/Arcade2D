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
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _knockbackForce = 8f;
    [SerializeField] private float _hitStunDuration = 0.25f;
    [SerializeField] private float _attackDuration = 0.3f;
    
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private State _currentState;
    private InputController _inputController;
    private Health _health;
    private Trigger _invincible;
    private Sprite _lastSprite;
    private bool _hurtBoxActive;
    private bool _blockBoxActive;
    private readonly List<Vector2> _points = new();
    
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public float Speed => _speed;
    public float JumpForce => _jumpForce;
    public float AirControl => _airControl;
    public int Damage => _damage;
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
    
        SwitchState(new MoveState(gameObject));
    }
    
    private void Update() {
        _currentState.OnUpdate();
    
        if (_currentState is MoveState && _inputController.IsJumping && IsGrounded) {
            SwitchState(new JumpState(gameObject));
        }
    
        if (_currentState is JumpState && !_inputController.IsJumping && IsGrounded) {
            SwitchState(new MoveState(gameObject));
        }
    
        if (_currentState is MoveState && _inputController.IsAttacking.Value) {
            SwitchState(new AttackState(gameObject, _attackDuration));
        }
    
        if (_currentState is AttackState { IsFinished: true }) {
            SwitchState(new MoveState(gameObject));
        }
    
        if (_currentState is HitState { IsFinished: true }) {
            SwitchState(new MoveState(gameObject));
        }

        if (_currentState is BlockState { IsFinished: true })
        {
            SwitchState(new MoveState(gameObject));
        }
    }
    
    private void SwitchState(State state) {
        _currentState?.OnExit();
        _currentState = state;
        _currentState?.OnEnter();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("HurtBox")) return;
        if (_invincible.Value || _currentState is DeadState) return;
    
        PlayerController attacker = other.GetComponentInParent<PlayerController>();
        if (attacker == null || attacker == this) return;
    
        TakeHit(attacker);
    }
    
    private void TakeHit(PlayerController attacker) {
        _invincible.Launch();
    
        float side = Mathf.Sign(transform.position.x - attacker.transform.position.x);
        KnockbackDirection = new Vector2(side, 0.4f).normalized;
    
        bool isDead = _health.TakeDamage(attacker.Damage);
    
        if (isDead) SwitchState(new DeadState(gameObject));
        else SwitchState(new HitState(gameObject, _hitStunDuration));
    }
    
    public void OnAttackActive() {
        _hurtBoxActive = true;
        _lastSprite = null;
        _hurtBox.enabled = true;
    }
    
    public void OnAttackFinished() {
        _hurtBoxActive = false;
        _hurtBox.enabled = false;
    }

    public void OnBlockActive()
    {
        _blockBoxActive = true;
        _lastSprite = null;
        _blockBox.enabled = false;
    }
    
    public void OnBlockFinished()
    {
        _blockBoxActive = false;
        _lastSprite = null;
        _blockBox.enabled = true;
    }
    
    private void LateUpdate() {
        if (!_hurtBoxActive) return;
    
        Sprite sprite = _spriteRenderer.sprite;
        if (sprite == _lastSprite) return;
        _lastSprite = sprite;
    
        int shapeCount = sprite != null ? sprite.GetPhysicsShapeCount() : 0;
        _hurtBox.pathCount = shapeCount;
    
        for (int i = 0; i < shapeCount; i++) {
            sprite.GetPhysicsShape(i, _points);
    
            if (_spriteRenderer.flipX)
                for (int p = 0; p < _points.Count; p++)
                    _points[p] = new Vector2(-_points[p].x, _points[p].y);
    
            _hurtBox.SetPath(i, _points);
        }
    }

}