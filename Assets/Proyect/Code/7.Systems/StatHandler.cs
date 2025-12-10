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
        if(!gameObject.CompareTag("Player")) return;
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
            if (gameObject.CompareTag("Player"))
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

    public bool IsPlayerAlive() {
        if (Health > 0)
            return true;
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
        if (characterStatsSO != null)
            statsVars = characterStatsSO.statsVars;
        RaiseStatsEvent();
    }

    public void InflictDamage(int damage, GameObject damageSource) {
        Health -= damage;

        if (Health <= 0) {
            Die(damageSource);
        }
    }

    private void Die(GameObject killer) {
        // Determinar si es jugador o enemigo
        bool isPlayer = gameObject.CompareTag("Player");

        if (isPlayer) {
            // Evento de muerte del jugador
            PlayerDeathEvent deathEvent = new PlayerDeathEvent(killer, gameObject);
            EventBus<PlayerDeathEvent>.Raise(deathEvent);
        }
        else {
            // Evento de muerte de enemigo
            EnemyDeathEvent deathEvent = new EnemyDeathEvent(killer, gameObject);
            EventBus<EnemyDeathEvent>.Raise(deathEvent);
        }

        // Lógica común de muerte
        Debug.Log($"{gameObject.name} died by {killer?.name}");
    }
}
