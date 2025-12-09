using UnityEngine;

public class Tank : Agent {
    protected override void Start() {
        SetBehavior(new PursuitBehaviour());
    }

    protected override void ExecuteBehaviour() {
        FacePlayer();
        if (DistanceFromPlayer() <= 1.5f) {
            if (!OnAttackCoolDown()) {
                base.InvokeOnAttack();
            }
            m_rb.linearVelocity = Vector3.zero;
        }
        else {
            m_currentBehaviour?.Execute(this);
            m_rb.linearVelocity = m_currentVel;
        }
    }
}
