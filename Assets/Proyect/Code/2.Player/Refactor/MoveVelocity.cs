using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveVelocity : MonoBehaviour {
    private Vector3 velocityVector;
    private Rigidbody rb;
    private float moveSpeed;
    private bool isDashing;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (isDashing) return;
        rb.linearVelocity = velocityVector * moveSpeed;
    }

    public void StartDash(float dashSpeed) {
        isDashing = true;
        Vector3 dashDir = velocityVector != Vector3.zero ? velocityVector : transform.forward;
        float finalSpeed = velocityVector != Vector3.zero ? dashSpeed * 2f : dashSpeed;
        rb.linearVelocity = dashDir * finalSpeed;
    }

    public void StopDash() {
        isDashing = false;
        rb.linearVelocity = Vector3.zero;
    }

    #region Setters & Getters
    public void SetVelocity(Vector3 velocity) => velocityVector = velocity;
    public void SetSpeed(float speed) => moveSpeed = speed;
    public Vector3 GetVelocity() => velocityVector;
    public bool IsDashing => isDashing;
    #endregion
}
