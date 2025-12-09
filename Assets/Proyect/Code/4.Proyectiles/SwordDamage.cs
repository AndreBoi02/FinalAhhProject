using UnityEngine;

public class SwordDamage : MonoBehaviour {
    [SerializeField] int substractingVal;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<StatHandler>()) {
            other.GetComponent<StatHandler>().Health -= substractingVal;
            Debug.Log($"Agent hit: {other.gameObject.name}, damage dealt: {substractingVal}");
        }
    }
}
