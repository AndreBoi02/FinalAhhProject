using UnityEngine;

public class Ranger : Agent {

    protected override void Start() {
        SetBehavior(new FleeBehaviour());
    }

    protected override void Move() {
        if (DistanceFromPlayer() >= 7f) {
            if (!OnAttackCoolDown()) {
                FacePlayer();
                attackSystem.ExecuteAttack();
            }
            m_rb.linearVelocity = Vector3.zero;
        }
        else {
            m_currentBehaviour?.Execute(this);
            m_rb.linearVelocity = m_currentVel;
        }
    }
}
