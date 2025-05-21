using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Proyectile : MonoBehaviour {
    Rigidbody rb;
    public int Speed { get; private set; }
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Vector3 dirFinal;

    void Start() {
        rb = GetComponent<Rigidbody>();
        Speed = 10;
        dirFinal = (dir - transform.position).normalized;
    }

    void Update() {
        rb.linearVelocity = Speed * dirFinal;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<IDamageable>() != null) {
            collision.gameObject.GetComponent<IDamageable>().Damage();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
