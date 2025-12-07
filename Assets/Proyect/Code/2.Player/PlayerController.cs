using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static CombatEvent;

namespace FinalProyect {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour, IAttackSystem {
        #region References

        [SerializeField] Rigidbody rb;
        [SerializeField] CapsuleCollider capsuleCollider;
        [SerializeField] AttackSystem m_attackSystem;
        [SerializeField] StatHandler statHandler;

        #endregion  

        #region RunTimeVar

        [Header("Movement Vars")]
        [SerializeField] int normalSpeed;
        [SerializeField] int runnigSpeed;
        [SerializeField] int walkinigSpeed;

        [SerializeField] float dashCountdown;

        public float DashCD {
            get => dashCountdown;
            set {
                dashCountdown = value;
                EventBus<DashEvent>.Raise(new DashEvent {
                    OnDashCDChanged = Mathf.Floor(dashCountdown * 100f) / 100f
                });
            }
        }

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
                dashCountdown = 0;
                StartCoroutine(StopDashAndInmunity(0.2f));
                StartCoroutine(SmoothDashCooldown(1.5f));
            }
        }

        IEnumerator StopDashAndInmunity(float delay) {
            if (capsuleCollider) {
                capsuleCollider.enabled = false;
            }
            yield return new WaitForSeconds(delay);
            if (rb) {
                rb.linearVelocity = movementDir * normalSpeed;
            }
            capsuleCollider.enabled = true;
        }

        IEnumerator SmoothDashCooldown(float totalCooldownTime) {
            float elapsedTime = 0f;

            while (elapsedTime < totalCooldownTime) {
                elapsedTime += Time.deltaTime;

                float progress = Mathf.Clamp01(elapsedTime / totalCooldownTime);
                float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

                DashCD = smoothProgress * totalCooldownTime;

                yield return null;
            }

            DashCD = totalCooldownTime;
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

            if (_isMouseDown)
                AttackSystem.SetWorldPosVector(worldPos);
        }

        void LookAtMouseDir(Vector3 worldPos) {
            Vector3 LookAt = worldPos - transform.position;
            LookAt.y = 0;
            transform.rotation = Quaternion.LookRotation(LookAt);
        }

        #endregion

        #region AttackMethods

        public event System.Action OnPrepareAttack;
        public event System.Action OnAttack;

        public AttackSystem AttackSystem => m_attackSystem;
        bool _isMouseDown = false;

        public void OnClickDonw(InputAction.CallbackContext context) {
            if (context.performed && AttackSystem.weapon == AttackSystem.AttackType.magic ||
                AttackSystem.weapon == AttackSystem.AttackType.range) {
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
                EventBus<OnNextWeapon>.Raise(new OnNextWeapon());
            }
        }

        public void PrevWeapon(InputAction.CallbackContext context) {
            if (context.performed) {
                EventBus<OnPrevWeapon>.Raise(new OnPrevWeapon());
            }
        }

        #endregion

        #region PotionsMethods

        public void UseHpPot(InputAction.CallbackContext context) {
            if (context.performed && statHandler.HpPotAvailable() && statHandler.Health != 100) {
                statHandler.HpPot -= 1;
                if(statHandler.Health >= 81) {
                    int wastedPot = ((int)statHandler.Health + 20) - 100;
                    statHandler.Health = 100;
                    print(wastedPot);
                }
                else {
                    statHandler.Health += 20;
                }
            }
        }

        public void UseManaPot(InputAction.CallbackContext context) {
            if (context.canceled && statHandler.ManaPotAvailable() && statHandler.Mana != 50) {
                statHandler.ManaPot -= 1;
                if (statHandler.Mana >= 36) {
                    int wastedManaPot = ((int)statHandler.Mana + 15) - 50;
                    statHandler.Mana = 50;
                    print(wastedManaPot);
                }
                else {
                    statHandler.Mana += 15;
                }
            }
        }

        #endregion
    }
}
