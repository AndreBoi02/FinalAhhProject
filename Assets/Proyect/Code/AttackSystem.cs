using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour {
    Action attackFunction;
    public enum AttackType {
        sword,
        bow,
        magic
    }
    [Header ("CurrentWeapon")]
    public AttackType weapon;
    int weaponIdx = 0;

    [Header("WeaponsReferences")]
    [SerializeField] GameObject sword;
    [SerializeField] GameObject proyectile;

    void Start() {
        SwitchBetweenWeapons();
        if (sword == null)
            return;
        sword?.SetActive(false);
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

    public void ExecuteAttack() {
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
        StartCoroutine(TurnOnAndOffTheSword());
    }

    IEnumerator TurnOnAndOffTheSword() {
        sword?.SetActive(true);
        yield return new WaitForSeconds(.5f);
        sword?.SetActive(false);
    }

    void bowAttack() {
        if (proyectile == null)
            return;
        GameObject tempProyectile;
        tempProyectile = Instantiate(proyectile, transform.position, transform.localRotation);
    }

    void MagicAttack() {
        Debug.Log("El hechicero con sus poderes!!!");
    }
}
