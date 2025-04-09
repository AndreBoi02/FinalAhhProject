using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    Rigidbody2D rb2D;
    public int Speed { get; private set; } 

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        Speed = 10;
    }

    void Update() {
        rb2D.linearVelocityY = Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<IDamageable>() != null) {
            collision.gameObject.GetComponent<IDamageable>().Damage();
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
}
