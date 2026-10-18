using UnityEngine;

public class SowrdAttackHandler : MonoBehaviour, IWeapon {
    [SerializeField] private GameObject _sword;
    [SerializeField] private float attackCooldown = 0.5f;

    private float lastAttackTime = -999f;
    
    public void Equip() {
        _sword?.SetActive(true);
    }

    public void Unequip() {
        _sword?.SetActive(false);
    }

    public void PrepareAttack() {
        ExecuteAttack();
    }

    public void ExecuteAttack() {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;

        print("Sword Attacking");
    }
}
