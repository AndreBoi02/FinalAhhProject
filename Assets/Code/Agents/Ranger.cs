using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class Ranger : Agent {
    public GameObject bullet;
    public float attackCoolDown;
    float attackCounter;

    protected override void Move() {
        if (DistanceFromPlayer() >= 7f) {
            if (!OnAttackCoolDown()) {
                Attack();
            }
            rb2D.linearVelocity = Vector3.zero;
        }
        else {
            EnemyBehaviour.flee(this);
            rb2D.linearVelocity = m_currentVel;
        }
        
    }

    float DistanceFromPlayer() {
        return Vector2.Distance(m_pos, m_targetPos);
    }

    private void Attack() {
        GameObject tempProyectile;
        tempProyectile = Instantiate(bullet);
        tempProyectile.GetComponent<Bullet>().dir = aTarget.transform.position;
        tempProyectile.transform.position = transform.position;
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
