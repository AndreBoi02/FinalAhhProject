using UnityEngine;

public class SwordAttackHandler : MonoBehaviour, IWeapon {
    [SerializeField] private GameObject _sword;
    [SerializeField] private float attackCooldown = 0.5f;

    private CharacterAnimationHandler _animHandler;
    private float lastAttackTime = -999f;

    public void Initialize(CharacterAnimationHandler animHandler) {
        _animHandler = animHandler;
    }
    
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

        _animHandler?.PlayAttack(0); // 0 = Sword
        print("Sword Attacking");
    }
}
