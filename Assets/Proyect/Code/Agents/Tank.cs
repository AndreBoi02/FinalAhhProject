using UnityEngine;

public class Tank : Agent {
    protected override void Move() {
        FacePlayer();
        if (DistanceFromPlayer() <= 1.5f) {
            if (!OnAttackCoolDown()) {
                attackSystem.ExecuteAttack();
            }
            rb.linearVelocity = Vector3.zero;
        }
        else {
            EnemyBehaviour.seek(this);
            rb.linearVelocity = m_currentVel;
        }
    }
}
