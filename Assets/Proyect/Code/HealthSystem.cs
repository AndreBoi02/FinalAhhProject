using UnityEngine;

public class HealthSystem : MonoBehaviour {
    [SerializeField] float health;
    public float Health {
        get => health;
        set => health = value;
    }

    void Update() {
        if(!ChechIfAlive())
            Destroy(gameObject);
    }

    public void TakeDamege(float val) {
        Health -= val;
    }

    public void Heal(float val) {
        Health += val;
        if (Health > 100)
            Health = 100;
    }
    
    bool ChechIfAlive() {
        return Health <= 0;
    }
}
