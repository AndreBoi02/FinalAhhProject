using System.Collections;
using UnityEngine;

public class Mage : Agent {
    ISteeringBehaviour m_steeringBehaviour;

    protected override void Start() {
        SetBehavior(new WanderBehaviour());
    }

    protected override void Move() {
        if (DistanceFromPlayer() >= 7f) {
            if (!OnAttackCoolDown()) {
                FacePlayer();
                AttackSystem.SetWorldPosVector(m_targetPos);
                MagicAttack();
            }
        }
    }

    void MagicAttack() {
        Debug.Log("MagicAttack");
        StartCoroutine(InvokeMagic());
    }

    IEnumerator InvokeMagic() {
        base.PrepareOnAttack();
        yield return new WaitForSeconds(.5f);
        base.InvokeOnAttack();
    }
}
