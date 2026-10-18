using UnityEngine;

public class FireBallAttackHandler : MonoBehaviour, IWeapon {
    [SerializeField] GameObject _tome;
    [SerializeField] GameObject _fireBall;
    [SerializeField] GameObject _fireBallPV;

    public void Equip() {
        _tome?.SetActive(true);
    }

    public void Unequip() {
        _tome?.SetActive(false);
    }

    public void PrepareAttack() {
        //TODO: Logic 4 prep FireBall
    }
    public void ExecuteAttack() {
        print("Spell Attacking");
    }
}
