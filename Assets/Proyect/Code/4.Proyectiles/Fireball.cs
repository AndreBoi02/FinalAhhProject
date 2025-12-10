using UnityEngine;

public class Fireball : MonoBehaviour {
    private Rigidbody rb;
    private float customGravity;
    private Vector3 targetPosition;
    private bool isLaunched = false;
    [SerializeField] GameObject HitBox;
    [SerializeField] int substractingVal;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] GameObject shooter;
    void Awake() {
        rb = GetComponent<Rigidbody>();
        HitBox.SetActive(false);
    }

    public void SetLayer(bool isPlayerProjectile) {
        gameObject.layer = isPlayerProjectile ?
            LayerMask.NameToLayer("PlayerProjectiles") :
            LayerMask.NameToLayer("EnemyProjectiles");
    }

    public void SetShooter(GameObject shooter) {
        this.shooter = shooter;
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
    private bool hasHitSomething = false;

    private void OnTriggerEnter(Collider other) {
        if (hasHitSomething) return;

        if (other.CompareTag("Floor")) {
            hasHitSomething = true;
            if (shooter != null && shooter.CompareTag("Player")) {
                PlayerAccuracyEvent missEvent = new PlayerAccuracyEvent(shooter, 0, 1, 0);
                EventBus<PlayerAccuracyEvent>.Raise(missEvent);
            }

            Destroy(gameObject, lifeTime);
            GetComponent<MeshRenderer>().enabled = false;
            isLaunched = false;
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
            HitBox.SetActive(true);
            return;
        }

        if (other.TryGetComponent<StatHandler>(out StatHandler targetStats)) {
            hasHitSomething = true;
            if (shooter != null && shooter.CompareTag("Player")) {
                PlayerAccuracyEvent hitEvent = new PlayerAccuracyEvent(shooter, 0, 0, 1);
                EventBus<PlayerAccuracyEvent>.Raise(hitEvent);
            }
            targetStats.InflictDamage(substractingVal, gameObject);
        }
    }
}
