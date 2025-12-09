using System.Collections;
using UnityEngine;

public class Mage : Agent {

    #region Enums

    public enum typeOfBehaviours {
        Seek,
        Flee,
        none
    }

    #endregion

    public typeOfBehaviours type = typeOfBehaviours.none;

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
        yield return new WaitForSeconds(.65f);
        InvokeOnAttack();
    }
}
