using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveVelocity), typeof(DashController))]
public class RPlayerController : MonoBehaviour {
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _runningSpeed;

    private MoveVelocity _moveVelocity;
    private DashController _dashController;

    private void Awake() {
        _moveVelocity = GetComponent<MoveVelocity>();
        _dashController = GetComponent<DashController>();
    }

    private void Start() {
        _moveVelocity.SetSpeed(_baseSpeed);
    }

    public void MovePlayer(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector2 inputDir = context.ReadValue<Vector2>();
            Vector3 direction = new Vector3(inputDir.x, 0, inputDir.y).normalized;
            _moveVelocity.SetVelocity(direction);
        }
        else if (context.canceled) {
            _moveVelocity.SetVelocity(Vector3.zero);
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
}
