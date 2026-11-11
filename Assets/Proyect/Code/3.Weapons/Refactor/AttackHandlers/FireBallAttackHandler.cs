using UnityEngine;

public class FireBallAttackHandler : MonoBehaviour, IWeapon {
    [Header("Visuals & Prefabs")]
    [SerializeField] private GameObject _tome;
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private GameObject _fireBallPV;

    [Header("Spell Settings")]
    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private float customGravity = -15f;
    [SerializeField] private int manaCost = 10;
    [SerializeField] private float attackCooldown = 0.8f;

    private CharacterAnimationHandler _animHandler;
    private StatHandler _statHandler;
    private RAttackSystem _attackSystem;
    private GameObject _activeIndicator;
    private bool _isCharging = false;
    private float lastAttackTime = -999f;

    private void Awake() {
        _statHandler = GetComponentInParent<StatHandler>();
        _attackSystem = GetComponentInParent<RAttackSystem>();
    }

    private void Update() {
        // While aiming/charging, keep the floor indicator positioned under the target point
        if (_activeIndicator != null && _attackSystem != null) {
            _activeIndicator.transform.position = _attackSystem.TargetPosition;
        }
    }

    public void Initialize(CharacterAnimationHandler animHandler) {
        _animHandler = animHandler;
    }

    public void Equip() {
        _tome?.SetActive(true);
    }

    public void Unequip() {
        _isCharging = false;
        _tome?.SetActive(false);
        _animHandler?.SetAiming(false);

        if (_activeIndicator != null) {
            Destroy(_activeIndicator);
            _activeIndicator = null;
        }
    }

    public void PrepareAttack() {
        if (Time.time < lastAttackTime + attackCooldown) return;
        if (_statHandler != null && !_statHandler.ManaAvailable()) return;

        _isCharging = true;
        _animHandler?.PlayAttack(2); // 2 = Magic / Fireball
        _animHandler?.SetAiming(true);

        // Spawn floor indicator if not already active
        if (_fireBallPV != null && _activeIndicator == null) {
            Vector3 targetPos = _attackSystem != null ? _attackSystem.TargetPosition : transform.position;
            _activeIndicator = Instantiate(_fireBallPV, targetPos, Quaternion.identity);
        }
    }

    public void ExecuteAttack() {
        bool isPlayer = CompareTag("Player") || (transform.root != null && transform.root.CompareTag("Player"));

        if (isPlayer && !_isCharging) return;
        if (Time.time < lastAttackTime + attackCooldown && !isPlayer) return;

        _isCharging = false;
        lastAttackTime = Time.time;

        _animHandler?.SetAiming(false);

        // Clean up floor indicator
        if (_activeIndicator != null) {
            Destroy(_activeIndicator);
            _activeIndicator = null;
        }

        if (_fireBall == null) {
            Debug.LogWarning("FireBallAttackHandler: _fireBall prefab is missing in Inspector!");
            return;
        }

        if (_statHandler != null && !_statHandler.ManaAvailable()) {
            return;
        }

        CastFireball();

        if (_statHandler != null) {
            _statHandler.Mana -= manaCost;
        }
    }

    private void CastFireball() {
        Vector3 targetPos = _attackSystem != null && _attackSystem.TargetPosition != Vector3.zero
            ? _attackSystem.TargetPosition
            : transform.position + transform.forward * 5f;

        GameObject fireballObj = Instantiate(_fireBall, transform.position, Quaternion.identity);
        Fireball fireball = fireballObj.GetComponent<Fireball>();

        bool isPlayer = CompareTag("Player") || (transform.root != null && transform.root.CompareTag("Player"));

        if (fireball != null) {
            fireball.LaunchTowards(targetPos, arcHeight, customGravity);
            fireball.SetLayer(isPlayer);
            fireball.SetShooter(isPlayer ? transform.root.gameObject : gameObject);
        }

        if (isPlayer) {
            GameObject shooterObj = transform.root.gameObject;
            EventBus<PlayerAccuracyEvent>.Raise(new PlayerAccuracyEvent {
                Shooter = shooterObj,
                ProyectilesTrown = 1,
                ProyectilesMissed = 0,
                ProyectilesHit = 0
            });
        }
    }
}
