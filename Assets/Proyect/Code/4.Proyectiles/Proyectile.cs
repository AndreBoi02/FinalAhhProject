 using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Proyectile : MonoBehaviour {
    Rigidbody rb => GetComponent<Rigidbody>();

    [SerializeField] int bulletSpeed;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] int substractingVal;

    // MÉTODO NUEVO: Para configurar dirección de disparo
    public void SetShootingDirection(Vector3 direction) {
        transform.forward = direction.normalized;
    }

    public void SetLayer(bool isPlayerProjectile) {
        gameObject.layer = isPlayerProjectile ?
            LayerMask.NameToLayer("PlayerProjectiles") :
            LayerMask.NameToLayer("EnemyProjectiles");
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

        if (other.GetComponent<StatHandler>()) {
            other.GetComponent<StatHandler>().Health -= substractingVal;
            Debug.Log($"Agent hit: {other.gameObject.name}, damage dealt: {substractingVal}");
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
