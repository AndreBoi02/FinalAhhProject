using System.Collections;
using UnityEngine;

public class Mage : Agent {
    #region Enums
    public enum typeOfBehaviours {
        Seek,
        Flee,
        none
    }
    public typeOfBehaviours type = typeOfBehaviours.none;
    #endregion

    IMageLevels difficultyStrategy;
    [SerializeField] Transform[] tpPoints;
    [SerializeField] StatHandler[] alliesStats;
    public Mage(IMageLevels mageLevel) {
        mageLevel = difficultyStrategy;
    }

    protected override void Start() {
        base.Start();
        SetBehavior(new SeekBehaviour());
        difficultyStrategy = new HardMage();
    }

    protected override void ExecuteBehaviour() {
        difficultyStrategy?.Execute(this);
    }

    public void Attack() {
        if (!OnAttackCoolDown()) {
            FacePlayer();
            AttackSystem.SetWorldPosVector(m_targetPos);
            StartCoroutine(InvokeMagic());
        }
    }

    [Header("Heal vars")]
    [SerializeField] int healSpellAmount;
    [SerializeField] int healAmount;

    public void HealAllies() {
        if(healSpellAmount < 0) return;
        print("Heal Done");
        GetStatHandler().Health = healAmount;
        healSpellAmount -= 1;
    }

    [Header("Teleport vars")]
    [SerializeField] int tpSpellAmount;
    public void TpAwayFromPlayer() {
        if (tpSpellAmount < 0) return;
        float furthestDistanceFromPlayer = 0;
        int idxTp = 0;
        for (int i =0; i < tpPoints.Length - 1; i++) {
            if (furthestDistanceFromPlayer < Vector3.Distance(GetTargetPos(), tpPoints[i].position)) {
                furthestDistanceFromPlayer = Vector3.Distance(GetTargetPos(), tpPoints[i].position);
                idxTp = i;
            }
        }
        transform.position = tpPoints[idxTp].position;
        tpSpellAmount -= 1;
    }

    IEnumerator InvokeMagic() {
        PrepareOnAttack();
        yield return new WaitForSeconds(.65f);
        InvokeOnAttack();
    }
}
