using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveVelocity : MonoBehaviour {
    private Vector3 velocityVector;
    private Rigidbody rb;
    float moveSpeed;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        rb.linearVelocity = velocityVector * moveSpeed;
    }

    #region Setters
    public void SetVelocity(Vector3 velocity) => velocityVector = velocity;
    public void SetSpeed(float speed) => moveSpeed = speed;
    #endregion
}
