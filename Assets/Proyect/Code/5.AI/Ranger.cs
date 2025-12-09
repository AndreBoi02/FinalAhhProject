using UnityEngine;

public class Ranger : Agent {

    protected override void Start() {
        SetBehavior(new EvadeBehaviour());
    }

    protected override void ExecuteBehaviour() {
        if (DistanceFromPlayer() >= 7f) {
            if (!OnAttackCoolDown()) {
                FacePlayer();
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
