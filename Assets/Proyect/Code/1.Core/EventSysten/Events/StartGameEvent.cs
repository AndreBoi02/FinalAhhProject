using UnityEngine;

public struct StartGameEvent : IEvent {
    public bool isGameStarted;
}

public struct GameOverEvent : IEvent {
    public bool isGameOver;
}

public struct PlayerDeathEvent : IEvent {
    public GameObject Killer;
    public GameObject Player;

    public PlayerDeathEvent(GameObject killer, GameObject player) {
        this.Killer = killer;
        this.Player = player;
    }
}

public struct EnemyDeathEvent : IEvent {
    public GameObject Killer;
    public GameObject Enemy;

    public EnemyDeathEvent(GameObject killer, GameObject enemy) {
        this.Killer = killer;
        this.Enemy = enemy;
    }
}

public struct PlayerAccuracyEvent : IEvent {
    public GameObject Shooter;
    public int ProyectilesTrown;
    public int ProyectilesMissed;
    public int ProyectilesHit;

    public PlayerAccuracyEvent(GameObject shooter,int thrown, int missed, int hit = 0) {
        this.Shooter = shooter;
        ProyectilesTrown = thrown;
        ProyectilesMissed = missed;
        ProyectilesHit = hit;
    }
}

public struct PotionEfficiencyEvent : IEvent {
    public GameObject User;
    public int StatsPointsConsumed;
    public int StatsPointsWasted;
    public string PotionType;

    public PotionEfficiencyEvent(GameObject user,int consumed, int wasted, string type = "HP") {
        this.User = user;
        StatsPointsConsumed = consumed;
        StatsPointsWasted = wasted;
        PotionType = type;
    }
}
