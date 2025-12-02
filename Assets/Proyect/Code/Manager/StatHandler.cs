using UnityEngine;

public class StatHandler : MonoBehaviour {
    [SerializeField] float health;
    [SerializeField] float mana;
    [SerializeField] float ammo;
    [SerializeField] float hpPotion;
    [SerializeField] float manaPotion;

    private void RaiseStatsEvent() {
        EventBus<StatsEvent>.Raise(new StatsEvent {
            health = health,
            mana = mana,
            ammo = ammo,
            hpPot = hpPotion,
            manaPot = manaPotion
        });
    }

    public float Health {
        get => health;
        set {
            health = value;
            RaiseStatsEvent();
        }
    }

    public float Mana {
        get => mana;
        set {
            mana = value;
            RaiseStatsEvent();
        }
    }

    public float HpPot {
        get => hpPotion;
        set {
            hpPotion = value;
            RaiseStatsEvent();
        }
    }

    public float ManaPot {
        get => manaPotion;
        set {
            manaPotion = value;
            RaiseStatsEvent();
        }
    }

    public float Ammo {
        get => ammo;
        set {
            ammo = value;
            RaiseStatsEvent();
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
        RaiseStatsEvent();
    }
}
