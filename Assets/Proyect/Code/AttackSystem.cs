using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour {
    Action attackFunction;
    public enum AttackType {
        sword,
        bow,
        magic
    }

    public AttackType weapon;
    int weaponIdx = 0;

    void Start() {
        SwitchBetweenWeapons();
    }

    void SwitchBetweenWeapons() {
        switch (weapon) {
            case AttackType.sword:
                attackFunction = SwordAttack;
                break;
            case AttackType.bow:
                attackFunction = bowAttack;
                break;
            case AttackType.magic:
                attackFunction = MagicAttack;
                break;
            default:
                break;  
        }
    }

    public void ExecuteAttack(InputAction.CallbackContext context) {
        if(context.performed)
            attackFunction();
    }

    public void NextWeapon(InputAction.CallbackContext context) {
        if (context.performed) {
            weaponIdx = (weaponIdx + 1) % 3;
            weapon = (AttackType)weaponIdx;
            SwitchBetweenWeapons();
        }
    }

    public void PrevWeapon(InputAction.CallbackContext context) {
        if (context.performed) {
            weaponIdx = (weaponIdx - 1 + 3) % 3;
            weapon = (AttackType)weaponIdx;
            SwitchBetweenWeapons();
        }
    }

    void SwordAttack() {
        Debug.Log("Swing");
    }

    void bowAttack() {
        Debug.Log("Pew Pew");
    }

    void MagicAttack() {
        Debug.Log("El hechicero con sus poderes!!!");
    }
}
