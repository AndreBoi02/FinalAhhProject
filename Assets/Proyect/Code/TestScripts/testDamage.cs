using UnityEngine;

public class testDamage : MonoBehaviour {
    [SerializeField] float substractingVal;
    [SerializeField] float addingVal;

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<StatHandler>()) {
            print(other.gameObject.name);
            other.GetComponent<StatHandler>().Health -= substractingVal;
        }
    }
}
