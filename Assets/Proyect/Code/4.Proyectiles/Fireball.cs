using UnityEngine;

public class Fireball : MonoBehaviour {
    private Rigidbody rb;
    private float customGravity;
    private Vector3 targetPosition;
    private bool isLaunched = false;
    [SerializeField] GameObject HitBox;
    [SerializeField] int substractingVal;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        HitBox.SetActive(false);
    }

    public void SetLayer(bool isPlayerProjectile) {
        gameObject.layer = isPlayerProjectile ?
            LayerMask.NameToLayer("PlayerProjectiles") :
            LayerMask.NameToLayer("EnemyProjectiles");
    }

    public void LaunchTowards(Vector3 target, float arcHeight, float gravity) {
        targetPosition = target;
        customGravity = gravity;

        Vector3 directionToTarget = target - transform.position;
        float horizontalDistance = new Vector3(directionToTarget.x, 0, directionToTarget.z).magnitude;

        float time = Mathf.Sqrt(-2 * arcHeight / gravity) + Mathf.Sqrt(2 * (directionToTarget.y - arcHeight) / gravity);
        float horizontalVelocity = horizontalDistance / time;
        float verticalVelocity = Mathf.Sqrt(-2 * gravity * arcHeight);

        Vector3 velocity = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized * horizontalVelocity;
        velocity.y = verticalVelocity;

        rb.linearVelocity = velocity;
        isLaunched = true;
    }

    void FixedUpdate() {
        if (isLaunched) {
            rb.linearVelocity += Vector3.up * customGravity * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Floor")) {
            GetComponent<MeshRenderer>().enabled = false;
            isLaunched = false;
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            HitBox.SetActive(true);
        }
        if (other.GetComponent<StatHandler>()) {
            other.GetComponent<StatHandler>().Health -= substractingVal;
            Debug.Log($"Agent hit: {other.gameObject.name}, damage dealt: {substractingVal}");
        }
    }
}
