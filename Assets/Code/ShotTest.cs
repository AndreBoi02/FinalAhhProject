using UnityEngine;
using UnityEngine.InputSystem;

public class ShotTest : MonoBehaviour {
    [SerializeField] GameObject _proyectile;
    public void Shoot(InputAction.CallbackContext context) {
        if (context.performed) {
            GameObject tempProyectile;
            tempProyectile = Instantiate(_proyectile);
            tempProyectile.transform.position = transform.position;
        }
    }
}
