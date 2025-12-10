using UnityEngine;

[System.Serializable]
public struct DataToMine {
    [Tooltip("Total Damage done by the player, at the end of the lvl")]
    [SerializeField] public float totalDamageFromPlayer;
    [Tooltip("Total Damage recived by the player, at the end of the lvl")]
    [SerializeField] public float totalDamageToPlayer;

    [Tooltip("Damage that could've been dodged")]
    [SerializeField] public float avoidableDamage;
    [Tooltip("Damage that killed the player but did the plater died with pots or dash available")]
    [SerializeField] public float criticalAvoidableDamage;

    [Tooltip("Accuracy of the projectiles thrown by the player")]
    [SerializeField] public float accuracy;
    [Tooltip("How efficient is the player with his pots")]
    [SerializeField] public float potionEfficiency;

    [Tooltip("Cosntant damage done by the player, without getting hit")]
    [SerializeField] public float dps;

    [Tooltip("Time taken by the player to clear the level")]
    [SerializeField] public float timeTakenToClearLvl;

    [Header("Log of enemies killed")]
    [SerializeField] public float meleeEnemiesKilled;
    [SerializeField] public float rangerEnemiesKilled;
    [SerializeField] public float mageEnemiesKilled;

    [Header("Log of the enemies kills")]
    [SerializeField] public float meleeEnemieKillCount;
    [SerializeField] public float rangerEnemieKillCount;
    [SerializeField] public float mageEnemieKillCount;
}

public class PlayerData : MonoBehaviour {
    public DataToMine dataToMine;
}
