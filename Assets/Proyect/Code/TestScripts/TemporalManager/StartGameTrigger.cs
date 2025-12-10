using UnityEngine;

public class StartGameTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            print($"Game Has Started");
            EventBus<StartGameEvent>.Raise(new StartGameEvent {
                isGameStarted = true
            });
        }
    }
}
