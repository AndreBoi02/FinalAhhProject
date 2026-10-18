using UnityEngine;

public class RAttackSystem : MonoBehaviour {
    private IWeapon weapon;

    private void Awake() {
        InitializeWeapon();
    }

    private void InitializeWeapon() {
        weapon = GetComponent<IWeapon>();
        if (weapon == null) {
            Debug.Log($"ERROR-Valid weapon missing.");
        }
    }

    public void PrepareAttack() {
        if (weapon == null) return;
        weapon.PrepareAttack();
    }
    
    public void ExecuteAttack() {
        if (weapon == null) return;
        weapon.ExecuteAttack();
    }

    public void SetWeapon(IWeapon t_weapon) {
        weapon = t_weapon;
        Debug.Log($"New weapon set: {weapon}");
    }
}
