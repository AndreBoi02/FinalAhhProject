using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour {
    [SerializeField] private Animator m_Animator;

    private static readonly int IsDeadHash = Animator.StringToHash("isDead");
    private static readonly int IsMovingHash = Animator.StringToHash("isRunning");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int AttackTypeHash = Animator.StringToHash("AttackType");
    private static readonly int IsAimingHash = Animator.StringToHash("isAiming");

    public void SetMoving(bool isMoving) {
        m_Animator.SetBool(IsMovingHash, isMoving);
    }

    public void SetAiming(bool isAiming) {
        m_Animator.SetBool(IsAimingHash, isAiming);
    }

    public void PlayAttack(int attackType = 0) {
        m_Animator.SetInteger(AttackTypeHash, attackType);
        m_Animator.SetTrigger(AttackTriggerHash);
    }

    public void PlayDeath(bool isDead = true) {
        m_Animator.SetBool(IsDeadHash, isDead);
    }
}
