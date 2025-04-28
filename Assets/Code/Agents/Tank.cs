using System.Collections;
using UnityEngine;

public class Tank : Agent {
    public float attackCoolDown;
    float attackCounter;
    public GameObject meleeAttackBoxCollider;

    protected override void Start() {
        base.Start();
        meleeAttackBoxCollider.SetActive(false);
    }

    protected override void Move() {
        if (DistanceFromPlayer() <= 1.5f) {
            if (!OnAttackCoolDown()) {
                StartCoroutine("Attack");
            }
            rb2D.linearVelocity = Vector3.zero;
        }
        else {
            EnemyBehaviour.seek(this);
            rb2D.linearVelocity = m_currentVel;
        }

    }

    float DistanceFromPlayer() {
        return Vector2.Distance(m_pos, m_targetPos);
    }

    IEnumerator Attack() {
        meleeAttackBoxCollider.SetActive(true);
        yield return new WaitForSeconds(.5f);
        meleeAttackBoxCollider.SetActive(false);
    }

    bool OnAttackCoolDown() {
        if (attackCounter > attackCoolDown) {
            attackCounter = 0;
            return false;
        }
        attackCounter += Time.deltaTime;
        return true;
    }
}
