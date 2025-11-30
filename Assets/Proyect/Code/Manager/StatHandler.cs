using System;
using UnityEngine;

public class StatHandler : MonoBehaviour {
    public event Action<float> OnHpChanged;
    public event Action<float> OnManaChanged;
    public event Action<float> OnAmmoChanged;
    public event Action<float> OnHpPotChanged;
    public event Action<float> OnManaPotChanged;

    [SerializeField] float health;
    [SerializeField] float mana;
    [SerializeField] float ammo;
    [SerializeField] float hpPotion;
    [SerializeField] float manaPotion;

    public float Health {
        get => health;
        set {
            health = value;
            OnHpChanged?.Invoke(health);
        }
    }

    public float Mana {
        get => mana;
        set {
            mana = value;
            OnManaChanged?.Invoke(mana);
        }
    }

    public float HpPot {
        get => hpPotion;
        set {
            hpPotion = value;
            OnHpPotChanged?.Invoke(hpPotion);
        }
    }

    public float ManaPot {
        get => manaPotion;
        set {
            manaPotion = value;
            OnManaPotChanged?.Invoke(manaPotion);
        }
    }

    public float Ammo {
        get => ammo;
        set {
            ammo = value;
            OnAmmoChanged?.Invoke(ammo);
        }
    }

    public bool ManaAvailable() {
        if (Mana > 0) {
            return true;
        }
        return false;
    }

    public bool AmmoAvailable() {
        if (Ammo > 0) {
            return true;
        }
        return false;
    }

    public bool PlayerAlive() {
        if (Health > 0) {
            return true;
        }
        return false;
    }

    public bool HpPotAvailable() {
        if (HpPot > 0) {
            return true;
        }
        return false;
    }

    public bool ManaPotAvailable() {
        if (ManaPot > 0) {
            return true;
        }
        return false;
    }

    private void Start() {
        OnHpChanged?.Invoke(Health);
        OnManaChanged?.Invoke(Mana);
        OnHpPotChanged?.Invoke(HpPot);
        OnManaPotChanged?.Invoke(ManaPot);
        OnAmmoChanged?.Invoke(Ammo);
    }
}
