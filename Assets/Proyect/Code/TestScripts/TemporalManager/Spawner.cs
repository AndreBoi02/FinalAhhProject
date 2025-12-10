using System.Collections.Generic;
using UnityEngine;
using static DataAnalyzer;

public class Spawner : MonoBehaviour {
    [System.Serializable]
    public class EnemyUnlock {
        public DataAnalyzer.PlayerProfile requiredProfile;
        public GameObject enemyPrefab;
        public float spawnChance = 0.3f;
        public bool isUnlocked = false;
    }

    [SerializeField] private List<EnemyUnlock> enemyUnlocks = new List<EnemyUnlock>();
    [SerializeField] private DataAnalyzer dataAnalyzer;

    private EventBinding<ProfileChangedEvent> profileBinding;

    private void Start() {
        if (dataAnalyzer == null) {
            dataAnalyzer = GetComponent<DataAnalyzer>();
        }

        if (dataAnalyzer == null) {
            Debug.LogError("Spawner: No se encontró DataAnalyzer!");
            return;
        }

        profileBinding = new EventBinding<ProfileChangedEvent>(OnProfileChanged);
        EventBus<ProfileChangedEvent>.Register(profileBinding);

        // Inicializar unlocks
        UpdateUnlocks(dataAnalyzer.GetCurrentProfile());
    }

    private void OnDisable() {
        if (profileBinding != null) {
            EventBus<ProfileChangedEvent>.Deregister(profileBinding);
        }
    }

    private void OnProfileChanged(ProfileChangedEvent e) {
        UpdateUnlocks(e.NewProfile);
    }

    private void UpdateUnlocks(DataAnalyzer.PlayerProfile profile) {
        foreach (var unlock in enemyUnlocks) {
            if ((int)profile >= (int)unlock.requiredProfile) {
                unlock.isUnlocked = true;
                Debug.Log($"Enemigo desbloqueado: {unlock.enemyPrefab.name}");
            }
        }
    }

    public GameObject GetRandomEnemy() {
        List<GameObject> availableEnemies = new List<GameObject>();

        foreach (var unlock in enemyUnlocks) {
            if (unlock.isUnlocked && Random.value <= unlock.spawnChance) {
                availableEnemies.Add(unlock.enemyPrefab);
            }
        }

        if (availableEnemies.Count == 0) {
            // Devolver enemigo base si no hay unlocks
            return enemyUnlocks[0].enemyPrefab;
        }

        return availableEnemies[Random.Range(0, availableEnemies.Count)];
    }
}
