using UnityEngine;

public class MouseOrientation : MonoBehaviour {
    [SerializeField] private Camera _cam;

    private RAttackSystem _attackSystem;
    private Vector3 worldPos;

    public Vector3 WorldPos => worldPos;

    private void Awake() {
        _attackSystem = GetComponent<RAttackSystem>();
        if (_cam == null) _cam = Camera.main;
    }

    private Vector3 GetMouseWorldPosition() {
        if (_cam == null) return Vector3.zero;

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, LayerMask.GetMask("Floor"))) {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void FixedUpdate() {
        worldPos = GetMouseWorldPosition();
        LookAtMouseDir(worldPos);

        if (_attackSystem != null) {
            _attackSystem.TargetPosition = worldPos;
        }
    }

    private void LookAtMouseDir(Vector3 targetPos) {
        Vector3 lookAt = targetPos - transform.position;
        lookAt.y = 0;
        if (lookAt != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(lookAt);
        }
    }
}
