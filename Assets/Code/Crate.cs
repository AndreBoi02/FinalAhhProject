using UnityEngine;

public class Crate : MonoBehaviour, IDamageable {
    public void Damage() {
        Debug.Log("Crate: You hit me >:V");
    }
}
