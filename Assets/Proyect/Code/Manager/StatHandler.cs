using System;
using UnityEngine;

public class StatHandler : MonoBehaviour {
    public event Action<float> OnHpChanged;
    public event Action<float> OnManaChanged;
    public event Action<float> OnAmmoChanged;

    [SerializeField] float health;
    [SerializeField] float mana;
    [SerializeField] float ammo;

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

    public float Ammo {
        get => ammo;
        set {
            ammo = value;
            OnAmmoChanged?.Invoke(ammo);
        }
    }
}
