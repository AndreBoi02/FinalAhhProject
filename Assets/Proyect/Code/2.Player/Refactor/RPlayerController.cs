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

    private void Awake() {
        if (_moveVelocity == null) {
            _moveVelocity = GetComponent<MoveVelocity>();
        }
        if (_dashController == null) {
            _dashController = GetComponent<DashController>();
        }
        if(_potionHandler == null) {
            _potionHandler = GetComponent<PotionHandler>();
        }
        if(_characterAnimationHandler == null) {
            _characterAnimationHandler = GetComponent<CharacterAnimationHandler>();
        }
    }

    private void Start() {
        _moveVelocity.SetSpeed(_baseSpeed);
    }

    public void MovePlayer(InputAction.CallbackContext context) {
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
        if (context.performed) {
            _dashController.Dash();
        }
    }

    public void UseHpPot(InputAction.CallbackContext context) {
        if (context.performed) {
            _potionHandler.UseHpPot();
        }
    }

    public void UseManaPot(InputAction.CallbackContext context) {
        if (context.performed) {
            _potionHandler.UseManaPot();
        }
    }

    private bool _isHoldingAttack = false;

    public void NextWeapon(InputAction.CallbackContext context) {
        if (context.performed) {
            _isHoldingAttack = false;
            _equipmentHandler.NextWeapon();
        }
    }

    public void PrevWeapon(InputAction.CallbackContext context) {
        if (context.performed) {
            _isHoldingAttack = false;
            _equipmentHandler.PrevWeapon();
        }
    }
    
    public void holdClickPrepareAttack(InputAction.CallbackContext context) {
        if (context.performed) {
            _isHoldingAttack = true;
            _attackSystem.PrepareAttack();
        }
    }

    public void SingleClickAttack(InputAction.CallbackContext context) {
        if (context.canceled) {
            if (!_isHoldingAttack) return;
            _isHoldingAttack = false;
            _attackSystem.ExecuteAttack();
        }
    }
}
