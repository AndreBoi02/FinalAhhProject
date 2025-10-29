using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTest : MonoBehaviour {
    [SerializeField] Camera _cam;
    [SerializeField] GameObject _fireBallPV;
    [SerializeField] GameObject _fireBall;

    Vector3 lastValidPosition;
    bool _isMouseDown = false;
    GameObject tempObject;

    public void CreateBallPreview(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector3 worldPos = GetMouseWorldPosition();
            if (worldPos != Vector3.zero) {
                tempObject = Instantiate(_fireBallPV, worldPos, Quaternion.identity);
                _isMouseDown = true;
                lastValidPosition = worldPos;
            }
        }
    }

    private void FixedUpdate() {
        if (_isMouseDown && tempObject != null) {
            Vector3 worldPos = GetMouseWorldPosition();
            if (worldPos != Vector3.zero) {
                tempObject.transform.position = worldPos;
                lastValidPosition = worldPos;
            }
        }
    }

    Vector3 GetMouseWorldPosition() {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, LayerMask.GetMask("Floor"))) {
            return hit.point;
        }

        return Vector3.zero;
    }
    
    public void FireBall(InputAction.CallbackContext context) {
        if (context.canceled && _isMouseDown) {
            _isMouseDown = false;

            if (tempObject != null) {
                Destroy(tempObject);
            }
            CreateFireBall();
        }
    }

    void CreateFireBall() {
        GameObject _tempfb = Instantiate(_fireBall, transform.position, Quaternion.identity);
        Fireball fireball = _tempfb.GetComponent<Fireball>();

        if (fireball != null) {
            fireball.LaunchTowards(lastValidPosition, 3, -15f);
        }
    }
}
