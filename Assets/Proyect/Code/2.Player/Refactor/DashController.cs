using UnityEngine;
using System.Collections;

public class DashController : MonoBehaviour {
    private MoveVelocity _moveVelocity;

    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCd = 1.5f;
    [SerializeField] private float dashTimer;
    private bool canDash = true;

    private void Awake() {
        _moveVelocity = GetComponent<MoveVelocity>();
    }

    public void Dash() {
        if(!canDash) return;
        canDash = false;
        _moveVelocity.StartDash(dashSpeed);
        dashTimer = 0;
        StartCoroutine(StopDashAndInmunity(0.2f));
        StartCoroutine(SmoothDashCooldown(dashCd));
    }

    private IEnumerator StopDashAndInmunity(float delay) {
        yield return new WaitForSeconds(delay);
        _moveVelocity.StopDash();
    }

    private IEnumerator SmoothDashCooldown(float totalCooldownTime) {
        float elapsedTime = 0f;

        while (elapsedTime < totalCooldownTime) {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / totalCooldownTime);
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            dashTimer = smoothProgress * totalCooldownTime;

            yield return null;
        }

        dashTimer = totalCooldownTime;
        canDash = true;
    }
}
