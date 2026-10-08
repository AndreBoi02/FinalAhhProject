using UnityEngine;

public class MouseOrientation : MonoBehaviour {
    [SerializeField] Camera _cam;

    Vector3 GetMouseWorldPosition() {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, LayerMask.GetMask("Floor"))) {
            return hit.point;
        }
        return Vector3.zero;
    }

    Vector3 worldPos;
    private void FixedUpdate() {
        worldPos = GetMouseWorldPosition();
        LookAtMouseDir(worldPos);
    }

    void LookAtMouseDir(Vector3 worldPos) {
        Vector3 LookAt = worldPos - transform.position;
        LookAt.y = 0;
        transform.rotation = Quaternion.LookRotation(LookAt);
    }
}
