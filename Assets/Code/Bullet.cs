using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    Rigidbody2D rb2D;
    public int Speed { get; private set; }
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Vector3 dirFinal;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        Speed = 10;
        dirFinal = (dir - transform.position).normalized;
    }

    void Update() {
        rb2D.linearVelocity = Speed * dirFinal;
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
