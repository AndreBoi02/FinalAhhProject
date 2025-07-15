using UnityEngine;

public class Ranger : Agent {

    protected override void Move() {
        if (DistanceFromPlayer() >= 7f) {
            if (!OnAttackCoolDown()) {
                FacePlayer();
                attackSystem.ExecuteAttack();
            }
            m_rb.linearVelocity = Vector3.zero;
        }
        else {
            EnemyBehaviour.Flee(this);
            m_rb.linearVelocity = m_currentVel;
        }
    }
}
