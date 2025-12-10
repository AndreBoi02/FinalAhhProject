using UnityEngine;

public class SwordDamage : MonoBehaviour {
    [SerializeField] int substractingVal;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<StatHandler>(out StatHandler targetStats)) {
            targetStats.InflictDamage(substractingVal, gameObject);
        }
    }
}
