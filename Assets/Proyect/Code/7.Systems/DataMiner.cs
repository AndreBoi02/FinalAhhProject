using UnityEngine;

public class DataMiner : MonoBehaviour {
    [Tooltip("Total Damage done by the player, at the end of the lvl")]
    float totalDamageFromPlayer;
    [Tooltip("Total Damage recived by the player, at the end of the lvl")]
    float totalDamageToPlayer;

    [Tooltip("Damage that could've been dodged")]
    float avoidableDamage;
    [Tooltip("Damage that killed the player but did the plater died with pots or dash available")]
    float criticalAvoidableDamage;

    [Tooltip("Accuracy of the projectiles thrown by the player")]
    float accuracy;
    [Tooltip("How efficient is the player with his pots")]
    float potionEfficiency;

    [Tooltip("Cosntant damage done by the player, without getting hit")]
    float dps;

    [Tooltip("Time taken by the player to clear the level")]
    float timeTakenToClearLvl;

    [Header("Log of enemies killed")]
    float meleeEnemiesKilled;
    float rangerEnemiesKilled;
    float mageEnemiesKilled;

    [Header("Log of the enemies kills")]
    float meleeEnemieKillCount;
    float rangerEnemieKillCount;
    float mageEnemieKillCount;
}
