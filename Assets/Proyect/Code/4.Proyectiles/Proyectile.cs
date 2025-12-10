 using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Proyectile : MonoBehaviour {
    Rigidbody rb => GetComponent<Rigidbody>();

    [SerializeField] int bulletSpeed;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] int substractingVal;
    [SerializeField] GameObject shooter;

    // MÉTODO NUEVO: Para configurar dirección de disparo
    public void SetShootingDirection(Vector3 direction) {
        transform.forward = direction.normalized;
    }

    public void SetLayer(bool isPlayerProjectile) {
        gameObject.layer = isPlayerProjectile ?
            LayerMask.NameToLayer("PlayerProjectiles") :
            LayerMask.NameToLayer("EnemyProjectiles");
    }

    public void SetShooter(GameObject shooter) {
        this.shooter = shooter;
    }

    public int Speed {
        get => bulletSpeed;
        set => bulletSpeed = value;
    }

    void Start() {
        Destroy(gameObject, lifeTime);
    }

    void Update() {
        rb.linearVelocity = transform.forward * Speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<StatHandler>(out StatHandler targetStats)) {
            if (shooter != null && shooter.CompareTag("Player")) {
                PlayerAccuracyEvent hitEvent = new PlayerAccuracyEvent(shooter,0, 0, 1);
                EventBus<PlayerAccuracyEvent>.Raise(hitEvent);
            }
            targetStats.InflictDamage(substractingVal, gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall")) {
            if (shooter != null && shooter.CompareTag("Player")) {
                PlayerAccuracyEvent missEvent = new PlayerAccuracyEvent(shooter, 0, 1, 0);
                EventBus<PlayerAccuracyEvent>.Raise(missEvent);
            }
            Destroy(gameObject);
        }
    }
}
