using UnityEngine;
using UnityEngine.InputSystem;

public class RPlayerController : MonoBehaviour {
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _runningSpeed;

    [SerializeField] private MoveVelocity _moveVelocity;
    [SerializeField] private DashController _dashController;
    [SerializeField] private PotionHandler _potionHandler;
    [SerializeField] private RAttackSystem _attackSystem;
    [SerializeField] private EquipmentHandler _equipmentHandler;
    [SerializeField] private CharacterAnimationHandler _characterAnimationHandler;

    private StatHandler _statHandler;
    private EventBinding<DeathEvent> _deathEventBinding;
    private bool _isDead = false;
    private bool _isHoldingAttack = false;

    private void Awake() {
        if (_moveVelocity == null) _moveVelocity = GetComponent<MoveVelocity>();
        if (_dashController == null) _dashController = GetComponent<DashController>();
        if (_potionHandler == null) _potionHandler = GetComponent<PotionHandler>();
        if (_characterAnimationHandler == null) _characterAnimationHandler = GetComponent<CharacterAnimationHandler>();
        _statHandler = GetComponent<StatHandler>();
    }

    private void OnEnable() {
        _deathEventBinding = new EventBinding<DeathEvent>(OnDeath);
        EventBus<DeathEvent>.Register(_deathEventBinding);
    }

    private void OnDisable() {
        if (_deathEventBinding != null) {
            EventBus<DeathEvent>.Deregister(_deathEventBinding);
        }
    }

    private void Start() {
        _moveVelocity.SetSpeed(_baseSpeed);
    }

    private void Update() {
        if (_statHandler != null) {
            _statHandler.PlayerAlive();
        }
    }

    private void OnDeath(DeathEvent deathEvent) {
        if (deathEvent.Source == gameObject || (deathEvent.Source != null && deathEvent.Source == transform.root.gameObject)) {
            _isDead = deathEvent.isDead;
            if (_isDead) {
                _moveVelocity.SetVelocity(Vector3.zero);
                _characterAnimationHandler?.SetMoving(false);
                _characterAnimationHandler?.PlayDeath(true);
            }
        }
    }

    public void MovePlayer(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            _characterAnimationHandler.SetMoving(true);
            Vector2 inputDir = context.ReadValue<Vector2>();
            Vector3 direction = new Vector3(inputDir.x, 0, inputDir.y).normalized;
            _moveVelocity.SetVelocity(direction);
        }
        else if (context.canceled) {
            _moveVelocity.SetVelocity(Vector3.zero);
            _characterAnimationHandler.SetMoving(false);
        }
    }

    public void ChangeSpeed(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            if (context.action.name == "Run") {
                _moveVelocity.SetSpeed(_runningSpeed);
            }
            else if (context.action.name == "Walk") {
                _moveVelocity.SetSpeed(_walkingSpeed);
            }
        }
        else if (context.canceled) {
            _moveVelocity.SetSpeed(_baseSpeed);
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            _dashController.Dash();
        }
    }

    public void UseHpPot(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            _potionHandler.UseHpPot();
        }
    }

    public void UseManaPot(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            _potionHandler.UseManaPot();
        }
    }

    public void NextWeapon(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            _isHoldingAttack = false;
            _equipmentHandler.NextWeapon();
        }
    }

    public void PrevWeapon(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            _isHoldingAttack = false;
            _equipmentHandler.PrevWeapon();
        }
    }
    
    public void holdClickPrepareAttack(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.performed) {
            _isHoldingAttack = true;
            _attackSystem.PrepareAttack();
        }
    }

    public void SingleClickAttack(InputAction.CallbackContext context) {
        if (_isDead) return;

        if (context.canceled) {
            if (!_isHoldingAttack) return;
            _isHoldingAttack = false;
            _attackSystem.ExecuteAttack();
        }
    }
}
