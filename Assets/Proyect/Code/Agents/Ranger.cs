using UnityEngine;

public class Ranger : Agent {
    protected override void Move() {
        if (DistanceFromPlayer() >= 7f) {
            if (!OnAttackCoolDown()) {
                FacePlayer();
                attackSystem.ExecuteAttack();
            }
            rb.linearVelocity = Vector3.zero;
        }
        else {
            EnemyBehaviour.flee(this);
            rb.linearVelocity = m_currentVel;
        }
    }
}
