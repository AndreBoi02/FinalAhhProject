using UnityEngine;

public class BowAttackHandler : MonoBehaviour, IWeapon {
    [SerializeField] GameObject _bow;
    [SerializeField] GameObject _proyectile;

    public void Equip() {
        _bow?.SetActive(true);
    }

    public void Unequip() {
        _bow?.SetActive(false);
    }

    public void PrepareAttack() {
        //TODO: Logic 4 prep Bow
    }

    public void ExecuteAttack() {
        print("Bow Attacking");
    }
}
