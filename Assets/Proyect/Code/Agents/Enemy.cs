using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {
    public void Damage() {
        Debug.Log("Enemy: You hit me :/");
    }
}
