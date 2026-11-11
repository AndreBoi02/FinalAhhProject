using UnityEngine;

public class RAttackSystem : MonoBehaviour {
    [SerializeField] private CharacterAnimationHandler _animHandler;
    private IWeapon weapon;

    public Vector3 TargetPosition { get; set; }

    private void Awake() {
        if (_animHandler == null) {
            _animHandler = GetComponent<CharacterAnimationHandler>();
        }
        InitializeWeapon();
    }

    private void InitializeWeapon() {
        weapon = GetComponent<IWeapon>();
        if (weapon == null) {
            Debug.Log($"ERROR-Valid weapon missing.");
            return;
        }
        weapon.Initialize(_animHandler);
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
        weapon?.Initialize(_animHandler);
        Debug.Log($"New weapon set: {weapon}");
    }

    public void SetTargetPosition(Vector3 pos) {
        TargetPosition = pos;
    }
}
