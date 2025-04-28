using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof (PlayerInput))]
public class PlayerController : MonoBehaviour {
    Rigidbody2D rb2D => GetComponent<Rigidbody2D>();

    public void Walk(InputAction.CallbackContext context) {

    }

    public void Attack() {

    }

    public void Dash() {

    }

    public void Run() {

    }
}
