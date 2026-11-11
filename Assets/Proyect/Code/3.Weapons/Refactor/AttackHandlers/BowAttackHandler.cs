using UnityEngine;

public class BowAttackHandler : MonoBehaviour, IWeapon {
    [Header("Visuals & Prefabs")]
    [SerializeField] private GameObject _bow;
    [SerializeField] private GameObject _proyectile;

    [Header("Bow Settings")]
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.5f, 1.5f);
    [SerializeField] private float attackCooldown = 0.5f;

    private CharacterAnimationHandler _animHandler;
    private StatHandler _statHandler;
    private RAttackSystem _attackSystem;
    private bool _isCharging = false;
    private float lastAttackTime = -999f;

    private void Awake() {
        _statHandler = GetComponentInParent<StatHandler>();
        _attackSystem = GetComponentInParent<RAttackSystem>();
    }

    public void Initialize(CharacterAnimationHandler animHandler) {
        _animHandler = animHandler;
    }

    public void Equip() {
        _bow?.SetActive(true);
    }

    public void Unequip() {
        _isCharging = false;
        _bow?.SetActive(false);
        _animHandler?.SetAiming(false);
    }

    public void PrepareAttack() {
        if (Time.time < lastAttackTime + attackCooldown) return;
        if (_statHandler != null && !_statHandler.AmmoAvailable()) return;

        _isCharging = true;
        _animHandler?.PlayAttack(1); // 1 = Bow
        _animHandler?.SetAiming(true);
    }

    public void ExecuteAttack() {
        bool isPlayer = CompareTag("Player") || (transform.root != null && transform.root.CompareTag("Player"));

        // If player didn't charge (e.g. cancelled mid-swap), abort
        if (isPlayer && !_isCharging) return;
        if (Time.time < lastAttackTime + attackCooldown && !isPlayer) return;

        _isCharging = false;
        lastAttackTime = Time.time;

        _animHandler?.SetAiming(false);

        if (_proyectile == null) {
            Debug.LogWarning("BowAttackHandler: _proyectile prefab is missing in Inspector!");
            return;
        }

        if (_statHandler != null && !_statHandler.AmmoAvailable()) {
            return;
        }

        ShootArrow();

        if (_statHandler != null) {
            _statHandler.Ammo -= 1;
        }
    }

    private void ShootArrow() {
        Vector3 spawnPosition = transform.position + transform.rotation * spawnOffset;
        Vector3 shootingDirection = transform.forward;

        bool isPlayer = CompareTag("Player") || (transform.root != null && transform.root.CompareTag("Player"));

        if (!isPlayer) {
            // AI Logic: Prediction based on Ranger level
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.TryGetComponent<Rigidbody>(out var playerRb)) {
                Ranger ranger = GetComponentInParent<Ranger>();
                bool shouldPredict = ranger == null || ranger.level == Ranger.Level.Hard;

                if (shouldPredict) {
                    shootingDirection = PredictionHelper.CalculateShootingDirection(
                        spawnPosition,
                        player.transform.position,
                        playerRb.linearVelocity,
                        projectileSpeed
                    );
                }
                else {
                    shootingDirection = (player.transform.position - spawnPosition).normalized;
                }
            }
        }
        else {
            // Player Logic: Shoot towards TargetPosition or Forward
            if (_attackSystem != null && _attackSystem.TargetPosition != Vector3.zero) {
                Vector3 toTarget = _attackSystem.TargetPosition - spawnPosition;
                toTarget.y = 0; // Keep arrow horizontal
                if (toTarget.sqrMagnitude > 0.01f) {
                    shootingDirection = toTarget.normalized;
                }
            }

            GameObject shooterObj = transform.root.gameObject;
            EventBus<PlayerAccuracyEvent>.Raise(new PlayerAccuracyEvent {
                Shooter = shooterObj,
                ProyectilesTrown = 1,
                ProyectilesMissed = 0,
                ProyectilesHit = 0
            });
        }

        // Spawn and configure projectile
        GameObject arrowObj = Instantiate(_proyectile, spawnPosition, Quaternion.LookRotation(shootingDirection));
        Proyectile arrow = arrowObj.GetComponent<Proyectile>();

        if (arrow != null) {
            arrow.Speed = (int)projectileSpeed;
            arrow.SetShootingDirection(shootingDirection);
            arrow.SetLayer(isPlayer);
            arrow.SetShooter(isPlayer ? transform.root.gameObject : gameObject);
        }
    }
}
