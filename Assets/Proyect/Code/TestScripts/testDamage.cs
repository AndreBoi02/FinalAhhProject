using UnityEngine;

public class testDamage : MonoBehaviour {
    [SerializeField] float substractingVal;
    [SerializeField] float addingVal;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<StatHandler>()) {
            other.GetComponent<StatHandler>().Health -= substractingVal;
        }
    }
}
