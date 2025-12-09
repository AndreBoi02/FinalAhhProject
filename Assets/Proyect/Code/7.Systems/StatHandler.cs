using UnityEngine;

#region Struct
[System.Serializable]
public struct StatsVars {
    [SerializeField] public float health;
    [SerializeField] public float mana;
    [SerializeField] public float ammo;
    [SerializeField] public float hpPotion;
    [SerializeField] public float manaPotion;
}
#endregion

public class StatHandler : MonoBehaviour {
    [SerializeField] Stats_SO characterStatsSO;
    [SerializeField] StatsVars statsVars;

    void RaiseStatsEvent() {
        EventBus<StatsEvent>.Raise(new StatsEvent {
            health = statsVars.health,
            mana = statsVars.mana,
            ammo = statsVars.ammo,
            hpPot = statsVars.hpPotion,
            manaPot = statsVars.manaPotion
        });
    }

    public float Health {
        get => statsVars.health;
        set {
            statsVars.health = value;
            if(gameObject.CompareTag("Player"))
                RaiseStatsEvent();
        }
    }

    public float Mana {
        get => statsVars.mana;
        set {
            statsVars.mana = value;
            if (gameObject.CompareTag("Player"))
                RaiseStatsEvent();
        }
    }

    public float HpPot {
        get => statsVars.hpPotion;
        set {
            statsVars.hpPotion = value;
            if (gameObject.CompareTag("Player"))
                RaiseStatsEvent();
        }
    }

    public float ManaPot {
        get => statsVars.manaPotion;
        set {
            statsVars.manaPotion = value;
            if (gameObject.CompareTag("Player"))
                RaiseStatsEvent();
        }
    }

    public float Ammo {
        get => statsVars.ammo;
        set {
            statsVars.ammo = value;
            if (gameObject.CompareTag("Player"))
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

    public void PlayerAlive() {
        if (Health > 0) return;
        EventBus<DeathEvent>.Raise(new DeathEvent {
            Source = gameObject,
            isDead = true
        });
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
        if (characterStatsSO != null)
            statsVars = characterStatsSO.statsVars;
        RaiseStatsEvent();
    }
}
