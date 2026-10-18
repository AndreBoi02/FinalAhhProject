using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RAttackSystem))]
public class EquipmentHandler : MonoBehaviour {
    private RAttackSystem _attackSystem;

    int weaponIdx = 0;
    private List<IWeapon> weapons = new List<IWeapon>();

    private void Awake() {
        _attackSystem = GetComponent<RAttackSystem>();
        InitializeWeaponsList();
    }

    private void Start() {
        if (weapons.Count > 0) {
            foreach (var w in weapons) w.Unequip();
            SetNewWeapon();
        }
    }

    private void InitializeWeaponsList() {
        weapons.Clear();

        IWeapon[] found = GetComponents<IWeapon>();

        foreach (IWeapon w in found) {
            if (w == null) continue;

            if (!weapons.Contains(w)) {
                weapons.Add(w);
                Debug.Log($"Weapon {w} added at index {weapons.Count - 1}");
            }
        }
    }

    public void NextWeapon() {
        if (weapons.Count == 0) return;
        weapons[weaponIdx].Unequip();
        weaponIdx = (weaponIdx + 1) % weapons.Count;
        SetNewWeapon();
    }

    public void PrevWeapon() {
        if (weapons.Count == 0) return;
        weapons[weaponIdx].Unequip();
        weaponIdx = (weaponIdx - 1 + weapons.Count) % weapons.Count;
        SetNewWeapon();
    }

    private void SetNewWeapon() {
        Debug.Log($"Swapped to {weapons[weaponIdx]}");
        weapons[weaponIdx].Equip();
        _attackSystem.SetWeapon(weapons[weaponIdx]);
    }
}
