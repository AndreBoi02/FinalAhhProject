using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalProyect {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {
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
                    transform.rotation = Quaternion.LookRotation(movementDir);
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
    }
}
