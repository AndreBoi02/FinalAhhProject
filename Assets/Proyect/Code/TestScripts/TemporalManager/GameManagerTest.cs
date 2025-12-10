using System.Collections.Generic;
using UnityEngine;
using static DataAnalyzer;

public class GameManagerTest : MonoBehaviour {
    [SerializeField] private DataMiner dataMiner;
    [SerializeField] private DataAnalyzer dataAnalyzer;
    [SerializeField] private Agent player;

    [System.Serializable]
    public class EnemyUnlock {
        public DataAnalyzer.PlayerProfile requiredProfile;
        public GameObject enemyPrefab;
        public string enemyName; // Para identificar
        public EnemyType enemyType;
        public Difficulty difficulty;
        public float spawnChance = 0.3f;
        [HideInInspector] public bool isUnlocked = false;
    }

    public enum EnemyType { Mage, Ranger, Tank }
    public enum Difficulty { Easy, Medium, Hard }

    [Header("Enemy Unlocks - ORDEN IMPORTANTE")]
    [Tooltip("Orden recomendado: Easy -> Medium -> Hard por cada tipo")]
    [SerializeField] List<EnemyUnlock> enemyUnlocks = new List<EnemyUnlock>();

    [Header("Spawn Settings")]
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] int maxEnemies = 10;
    [SerializeField] bool progressiveDifficulty = true;
    [SerializeField] float radius = 15f;

     EventBinding<ProfileChangedEvent> profileBinding;
     List<GameObject> activeEnemies = new List<GameObject>();

    private void Start() {
        if (dataMiner == null) dataMiner = GetComponent<DataMiner>();
        if (dataAnalyzer == null) dataAnalyzer = GetComponent<DataAnalyzer>();

        profileBinding = new EventBinding<ProfileChangedEvent>(OnProfileChanged);
        EventBus<ProfileChangedEvent>.Register(profileBinding);

        // Validar que tenemos enemigos configurados
        if (enemyUnlocks.Count == 0) {
            Debug.LogError("GameManager: No hay enemigos configurados en enemyUnlocks!");
        }

        UpdateUnlocks(dataAnalyzer.GetCurrentProfile());
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    private void OnProfileChanged(ProfileChangedEvent e) {
        UpdateUnlocks(e.NewProfile);
        Debug.Log($"Perfil cambiado: {e.OldProfile} -> {e.NewProfile}");
    }

    private void UpdateUnlocks(DataAnalyzer.PlayerProfile profile) {
        int unlockedCount = 0;

        foreach (var unlock in enemyUnlocks) {
            bool shouldUnlock = (int)profile >= (int)unlock.requiredProfile;

            if (shouldUnlock && !unlock.isUnlocked) {
                unlock.isUnlocked = true;
                unlockedCount++;
                Debug.Log($"Desbloqueado: {unlock.enemyName} ({unlock.difficulty})");
            }
        }

        if (unlockedCount > 0) {
            Debug.Log($"Total desbloqueados: {unlockedCount} nuevos enemigos");
        }
    }

    private void SpawnEnemy() {
        if (activeEnemies.Count >= maxEnemies) return;

        GameObject enemyPrefab = GetWeightedRandomEnemy();
        if (enemyPrefab == null) return;

        Vector3 spawnPos = GetRandomSpawnPosition();
        spawnPos.y = enemyPrefab.transform.position.y;
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.GetComponent<Agent>().SetTarget(player);
        activeEnemies.Add(enemy);

        // Opcional: Nombrar enemigo para debug
        enemy.name = $"{enemyPrefab.name}_Clone";
    }

    private GameObject GetWeightedRandomEnemy() {
        List<(GameObject prefab, float weight)> available = new List<(GameObject, float)>();

        foreach (var unlock in enemyUnlocks) {
            if (!unlock.isUnlocked) continue;

            float weight = unlock.spawnChance;

            // Ajustar peso por dificultad si progressiveDifficulty está activado
            if (progressiveDifficulty) {
                switch (unlock.difficulty) {
                    case Difficulty.Easy: weight *= 0.8f; break;   // Menos probable
                    case Difficulty.Medium: weight *= 1.0f; break; // Normal
                    case Difficulty.Hard: weight *= 1.2f; break;   // Más probable en perfiles altos
                }
            }

            available.Add((unlock.enemyPrefab, weight));
        }

        if (available.Count == 0) {
            // Fallback: primer enemigo de la lista
            return enemyUnlocks.Count > 0 ? enemyUnlocks[0].enemyPrefab : null;
        }

        // Selección por peso
        float totalWeight = 0;
        foreach (var item in available) {
            totalWeight += item.weight;
        }

        float random = Random.Range(0, totalWeight);
        float current = 0;

        foreach (var item in available) {
            current += item.weight;
            if (random <= current) {
                return item.prefab;
            }
        }

        return available[0].prefab;
    }

    private Vector3 GetRandomSpawnPosition() {
        // Ajusta según tu juego
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    public void OnEnemyDied(GameObject enemy) {
        activeEnemies.Remove(enemy);
        // Opcional: Evento de muerte para DataMiner
        // EventBus<EnemyDeathEvent>.Raise(new EnemyDeathEvent(...));
    }

    // Método para debug: Ver enemigos desbloqueados
    public void DebugUnlocks() {
        Debug.Log("=== ENEMIGOS DESBLOQUEADOS ===");
        foreach (var unlock in enemyUnlocks) {
            string status = unlock.isUnlocked ? "UnLocked" : "Locked";
            Debug.Log($"{status} {unlock.enemyName} - {unlock.difficulty} (Req: {unlock.requiredProfile})");
        }
    }

    // En Update para debug
    private void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            DebugUnlocks();
        }
    }
}
