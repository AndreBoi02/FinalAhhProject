 using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Proyectile : MonoBehaviour {
    Rigidbody rb => GetComponent<Rigidbody>();
    public int Speed {
        get => bulletSpeed; set {
            bulletSpeed = value;
        }
    }
    [SerializeField] int bulletSpeed;

    void Update() {
        rb.linearVelocity = transform.forward * Speed;
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.GetComponent<IDamageable>() != null) {
            collision.gameObject.GetComponent<IDamageable>().Damage();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall") ||
                collision.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
