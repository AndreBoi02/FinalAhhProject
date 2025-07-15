using UnityEngine;

public class Tank : Agent {

    protected override void Start() {
        SetBehavior(new SeekBehaviour());
    }

    protected override void Move() {
        FacePlayer();
        if (DistanceFromPlayer() <= 1.5f) {
            if (!OnAttackCoolDown()) {
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
