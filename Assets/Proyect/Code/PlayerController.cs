using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalProyect {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour, IAttackSystem {
        #region References

        Rigidbody rb => GetComponent<Rigidbody>();
        CapsuleCollider capsuleCollider => GetComponent<CapsuleCollider>();

        #endregion  

        #region RunTimeVar

        [Header("Movement Vars")]
        [SerializeField] int normalSpeed;
        [SerializeField] int runnigSpeed;
        [SerializeField] int walkinigSpeed;
        int initialSpeed;
        public int Speed {
            get => normalSpeed;
            set => normalSpeed = value;
        }
        Vector3 movementDir;

        [Header("Dash Var")]
        [SerializeField] int dashSpeed;
        bool canDash = true;

        #endregion

        #region UnityMethods

        void Start() {
            initialSpeed = Speed;
        }

        #endregion

        #region MovementMethods

        public void MovePlayer(InputAction.CallbackContext context) {
            if (context.performed) {
                if (context.action.name == "Move") {
                    movementDir = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y).normalized;
                    rb.linearVelocity = movementDir * normalSpeed;
                }
            }
            else {
                rb.linearVelocity = Vector3.zero;
            }
        }

        public void ChangeSpeed(InputAction.CallbackContext context) {
            if (context.control.IsPressed()) {
                if (context.action.name == "Run") {
                    normalSpeed = runnigSpeed;
                }
                else if (context.action.name == "Walk") {
                    normalSpeed = walkinigSpeed;
                }
            }
            else {
                normalSpeed = initialSpeed;
            }
        }

        #endregion

        #region DashMethods

        public void Dash(InputAction.CallbackContext context) {
            if (context.performed && canDash) {
                canDash = false;
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(movementDir * dashSpeed, ForceMode.Impulse);
                StartCoroutine(StopDashAndInmunity(0.2f));
                StartCoroutine(DashCooldown(1f));
            }
        }

        IEnumerator StopDashAndInmunity(float delay) {
            if (capsuleCollider) {
                capsuleCollider.enabled = false;
            } 
            yield return new WaitForSeconds(delay);
            if (rb) {
                rb.linearVelocity = Vector3.zero;
            }
            capsuleCollider.enabled = true;
        }

        IEnumerator DashCooldown(float time) {
            yield return new WaitForSeconds(time);
            canDash = true;
        }

        #endregion

        #region MouseMethods

        [SerializeField] Camera _cam;

        Vector3 GetMouseWorldPosition() {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10000f, LayerMask.GetMask("Floor"))) {
                return hit.point;
            }

            return Vector3.zero;
        }

        Vector3 worldPos;
        private void FixedUpdate() {
            worldPos = GetMouseWorldPosition();
            LookAtMouseDir(worldPos);

            if(_isMouseDown)
                AttackSystem.SetWorldPosVector(worldPos);
        }

        void LookAtMouseDir(Vector3 worldPos) {
            Vector3 LookAt = worldPos - transform.position;
            LookAt.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(LookAt);
        }

        #endregion

        #region AttackMethods

        public event System.Action OnPrepareAttack;
        public event System.Action OnAttack;
        public event System.Action OnNextWeapon;
        public event System.Action OnPrevWeapon;
        public AttackSystem AttackSystem => GetComponent<AttackSystem>();
        bool _isMouseDown = false;
        public void OnClickDonw(InputAction.CallbackContext context) {
            if (context.performed && AttackSystem.weapon == AttackSystem.AttackType.magic) {
                OnPrepareAttack?.Invoke();
                _isMouseDown = true;
            }
        }

        public void OnClickUp(InputAction.CallbackContext context) {
            if (context.canceled) {
                _isMouseDown = false;
                OnAttack?.Invoke();
            }
        }

        public void NextWeapon(InputAction.CallbackContext context) {
            if (context.performed) {
                OnNextWeapon?.Invoke();
                print("E");
            }
        }

        public void PrevWeapon(InputAction.CallbackContext context) {
            if (context.performed) {
                OnPrevWeapon?.Invoke();
                print("Q");
            }
        }

        #endregion
    }
}
