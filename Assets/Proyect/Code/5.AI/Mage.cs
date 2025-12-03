using System.Collections;
using UnityEngine;

public class Mage : Agent {
    protected override void Start() {
        SetBehavior(new WanderBehaviour());
    }

    protected override void Move() {
        if (DistanceFromPlayer() >= 7f) {
            if (!OnAttackCoolDown()) {
                FacePlayer();
                AttackSystem.SetWorldPosVector(m_targetPos);
                StartCoroutine(InvokeMagic());
            }
        }
    }

    IEnumerator InvokeMagic() {
        PrepareOnAttack();
        yield return new WaitForSeconds(.5f);
        InvokeOnAttack();
    }
}
