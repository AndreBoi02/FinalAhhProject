using UnityEngine;

public class DataMiner : MonoBehaviour {
    [SerializeField] GameObject player;
    #region UnityMethods
    // ===== MÉTODOS UNITY =====
    private void Start() {
        InitializeEventBindings();
        LoadSavedData(); // Cargar datos guardados
    }

    private void OnDisable() {
        UnsubscribeFromEvents();
        SaveAllData(); // Guardar al desactivar
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
        SaveAllData(); // Backup en destrucción
    }

    private void Update() {
        if (isCountingTime) {
            timePlayed += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F3)) {
            DisplayAllStatistics();
        }

        // Reset con F4
        if (Input.GetKeyDown(KeyCode.F4)) {
            ResetAllData();
        }

    }
    #endregion

        // ===== INICIALIZACIÓN =====
    private void InitializeEventBindings() {
        // Crear bindings
        startBinding = new EventBinding<StartGameEvent>(OnLevelStart);
        completeBinding = new EventBinding<GameOverEvent>(OnLevelComplete);
        playerDeathBinding = new EventBinding<PlayerDeathEvent>(OnPlayerDeath);
        enemyDeathBinding = new EventBinding<EnemyDeathEvent>(OnEnemyDeath);
        potionBinding = new EventBinding<PotionEfficiencyEvent>(OnPotionUsed);
        accuracyBinding = new EventBinding<PlayerAccuracyEvent>(OnPlayerAccuracy);

        // Suscribirse a eventos
        EventBus<StartGameEvent>.Register(startBinding);
        EventBus<GameOverEvent>.Register(completeBinding);
        EventBus<PlayerDeathEvent>.Register(playerDeathBinding);
        EventBus<EnemyDeathEvent>.Register(enemyDeathBinding);
        EventBus<PotionEfficiencyEvent>.Register(potionBinding);
        EventBus<PlayerAccuracyEvent>.Register(accuracyBinding);

        Debug.Log("DataMiner: Eventos suscritos correctamente");
    }

    private void UnsubscribeFromEvents() {
        EventBus<StartGameEvent>.Deregister(startBinding);
        EventBus<GameOverEvent>.Deregister(completeBinding);
        EventBus<PlayerDeathEvent>.Deregister(playerDeathBinding);
        EventBus<EnemyDeathEvent>.Deregister(enemyDeathBinding);
        EventBus<PotionEfficiencyEvent>.Deregister(potionBinding);
        EventBus<PlayerAccuracyEvent>.Deregister(accuracyBinding);

        Debug.Log("DataMiner: Eventos desuscritos");
    }

    #region TimeStuff
    EventBinding<StartGameEvent> startBinding;
    EventBinding<GameOverEvent> completeBinding;

    bool isTiming = false;
    float startTime = 0f;
    int totalTime = 0;


    private void OnLevelStart() {
        // Iniciar el cronómetro
        isTiming = true;
        startTime = Time.time; // Guardamos el momento de inicio
        isCountingTime = true;
        Debug.Log("Cronómetro iniciado");
    }

    private void OnLevelComplete() {
        if (!isTiming) return;

        // Calcular tiempo total
        float elapsedTime = Time.time - startTime;
        totalTime = Mathf.RoundToInt(elapsedTime);

        isTiming = false;
        isCountingTime = false;

        Debug.Log($"Tiempo para completar el nivel: {totalTime} segundos");

        // Aquí puedes guardar el dato, enviarlo a analytics, etc.
        SaveTimeData(totalTime);
    }

    private void SaveTimeData(int timeInSeconds) {
        // Guardar el tiempo (PlayerPrefs, base de datos, analytics, etc.)
        PlayerPrefs.SetInt("LastLevelTime", timeInSeconds);
        PlayerPrefs.Save();

        // También podrías llevar un promedio
        int totalAttempts = PlayerPrefs.GetInt("TotalAttempts", 0) + 1;
        int totalTimeAllAttempts = PlayerPrefs.GetInt("TotalTimeAllAttempts", 0) + timeInSeconds;

        PlayerPrefs.SetInt("TotalAttempts", totalAttempts);
        PlayerPrefs.SetInt("TotalTimeAllAttempts", totalTimeAllAttempts);
        PlayerPrefs.Save();
    }
    #endregion

    #region PlayerDeathStuff
    // ===== CONTADORES DE ESTADÍSTICAS =====
    // Muertes del jugador
    int playerDeathsCount = 0;

    // Enemigos eliminados por tipo
    int mageEnemiesKilled = 0;
    int rangerEnemiesKilled = 0;
    int tankEnemiesKilled = 0;

    // Quién mató al jugador (estadísticas opcionales)
    int killedByMageCount = 0;
    int killedByRangerCount = 0;
    int killedByTankCount = 0;

    EventBinding<PlayerDeathEvent> playerDeathBinding;
    EventBinding<EnemyDeathEvent> enemyDeathBinding;

    // ===== MANEJADORES DE EVENTOS =====
    private void OnPlayerDeath(PlayerDeathEvent deathEvent) {
        // Incrementar muertes del jugador
        playerDeathsCount++;

        // Registrar quién mató al jugador
        if (deathEvent.Killer != null) {
            RegisterPlayerKiller(deathEvent.Killer);
        }

        // Log para debug
        Debug.Log($"DataMiner: Jugador murió. Total: {playerDeathsCount}");

        // Guardar inmediatamente (opcional)
        SavePlayerDeathData();
    }

    private void RegisterPlayerKiller(GameObject killer) {
        string killerType = GetEnemyType(killer);

        switch (killerType) {
            case "Mage":
                killedByMageCount++;
                Debug.Log($"DataMiner: Jugador asesinado por Mage. Total: {killedByMageCount}");
                break;
            case "Ranger":
                killedByRangerCount++;
                Debug.Log($"DataMiner: Jugador asesinado por Ranger. Total: {killedByRangerCount}");
                break;
            case "Tank":
                killedByTankCount++;
                Debug.Log($"DataMiner: Jugador asesinado por Tank. Total: {killedByTankCount}");
                break;
        }
    }

    private void OnEnemyDeath(EnemyDeathEvent deathEvent) {
        if (deathEvent.Enemy == null) {
            Debug.LogWarning("DataMiner: EnemyDeathEvent sin enemigo asignado");
            return;
        }

        // Determinar tipo de enemigo y contar
        string enemyType = GetEnemyType(deathEvent.Enemy);

        switch (enemyType) {
            case "Mage":
                mageEnemiesKilled++;
                Debug.Log($"DataMiner: Mage eliminado. Total: {mageEnemiesKilled}");
                break;

            case "Ranger":
                rangerEnemiesKilled++;
                Debug.Log($"DataMiner: Ranger eliminado. Total: {rangerEnemiesKilled}");
                break;

            case "Tank":
                tankEnemiesKilled++;
                Debug.Log($"DataMiner: Tank eliminado. Total: {tankEnemiesKilled}");
                break;

            default:
                Debug.LogWarning($"DataMiner: Tipo de enemigo desconocido: {deathEvent.Enemy.tag}");
                break;
        }

        // Guardar inmediatamente (opcional)
        SaveEnemiesKilledData();
    }

    // ===== MÉTODOS AUXILIARES =====
    private string GetEnemyType(GameObject enemy) {
        if (enemy.CompareTag("Mage")) return "Mage";
        if (enemy.CompareTag("Ranger")) return "Ranger";
        if (enemy.CompareTag("Tank")) return "Tank";
        return "Unknown";
    }

    private void SavePlayerDeathData() {
        PlayerPrefs.SetInt("PlayerDeaths", playerDeathsCount);
        PlayerPrefs.SetInt("KilledByMage", killedByMageCount);
        PlayerPrefs.SetInt("KilledByRanger", killedByRangerCount);
        PlayerPrefs.SetInt("KilledByTank", killedByTankCount);
        PlayerPrefs.Save();
    }

    private void SaveEnemiesKilledData() {
        PlayerPrefs.SetInt("MageKills", mageEnemiesKilled);
        PlayerPrefs.SetInt("RangerKills", rangerEnemiesKilled);
        PlayerPrefs.SetInt("TankKills", tankEnemiesKilled);
        PlayerPrefs.Save();
    }

    public float GetKDRatio() {
        if (playerDeathsCount == 0) return GetTotalEnemiesKilled(); // Perfect KD
        return (float)GetTotalEnemiesKilled() / playerDeathsCount;
    }

    #endregion

    #region PotionsStuff
    int totalPotionPointsConsumed = 0;
    int totalPotionPointsWasted = 0;
    float potionEfficiency = 0;
    int hpPotionsUsed = 0;
    int manaPotionsUsed = 0;

    EventBinding<PotionEfficiencyEvent> potionBinding;

    private void OnPotionUsed(PotionEfficiencyEvent potionEvent) {
        if(potionEvent.User != player) return;
        // Acumular estadísticas
        totalPotionPointsConsumed += potionEvent.StatsPointsConsumed;
        totalPotionPointsWasted += potionEvent.StatsPointsWasted;

        // Contar por tipo (NO asumir todas son HP)
        if (potionEvent.PotionType == "HP") {
            hpPotionsUsed++;
        }
        else if (potionEvent.PotionType == "Mana") {
            manaPotionsUsed++;
        }
        else {
            hpPotionsUsed++; // Por defecto si no tiene tipo
        }

        potionEfficiency = GetPotionEfficiencyPercentage();

        Debug.Log($"Poción usada ({potionEvent.PotionType}). Total HP: {hpPotionsUsed}, Total Mana: {manaPotionsUsed}");
    }

    public float GetPotionEfficiencyPercentage() {
        int totalPoints = totalPotionPointsConsumed + totalPotionPointsWasted;
        if (totalPoints == 0) return 100f; // Si no usó pociones, es 100% eficiente

        return (totalPotionPointsConsumed / (float)totalPoints) * 100f;
    }
    #endregion

    #region ProjectileStuff

    int totalProjectilesThrown = 0;
    int projectilesMissed = 0;
    int projectilesHit = 0;
    float accruacyPercentage = 0;

    private EventBinding<PlayerAccuracyEvent> accuracyBinding;

    private void OnPlayerAccuracy(PlayerAccuracyEvent accuracyEvent) {
        print($"{accuracyEvent.Shooter} {player}");
        if(accuracyEvent.Shooter != player) return;
        totalProjectilesThrown += accuracyEvent.ProyectilesTrown;
        projectilesMissed += accuracyEvent.ProyectilesMissed;
        projectilesHit += accuracyEvent.ProyectilesHit;

        Debug.Log($"Accuracy Update - Tirados: {totalProjectilesThrown}, " +
                 $"Fallos: {projectilesMissed}, Aciertos: {projectilesHit}");
        accruacyPercentage = GetAccuracyPercentage();
    }

    public float GetAccuracyPercentage() {
        if (totalProjectilesThrown == 0) return 0f;
        return ((float)projectilesHit / totalProjectilesThrown) * 100f;
    }
    #endregion

    #region MoreStuff

    private float timePlayed = 0f;
    private bool isCountingTime = false;

    #region Nuevos Métodos Públicos para DataAnalyzer

    // Tiempo jugado
    public float GetTimePlayed() => timePlayed;

    // Pociones usadas
    public int GetTotalPotionsUsed() => hpPotionsUsed + manaPotionsUsed;
    public int GetHpPotionsUsed() => hpPotionsUsed;
    public int GetManaPotionsUsed() => manaPotionsUsed;

    // Enemigos eliminados total
    public int GetTotalEnemiesKilled() => mageEnemiesKilled + rangerEnemiesKilled + tankEnemiesKilled;

    // Muertes del jugador
    public int GetTotalPlayerDeaths() => playerDeathsCount;

    // Método para diferenciar tipos de enemigos eliminados
    public (int mage, int ranger, int tank) GetEnemiesKilledByType() {
        return (mageEnemiesKilled, rangerEnemiesKilled, tankEnemiesKilled);
    }

    // Método para saber quién mató al jugador
    public (int byMage, int byRanger, int byTank) GetPlayerDeathsByEnemyType() {
        return (killedByMageCount, killedByRangerCount, killedByTankCount);
    }

    #endregion

    #endregion

    #region Método para mostrar todas las estadísticas (útil para debug)

    public void DisplayAllStatistics() {
        Debug.Log("=== ESTADÍSTICAS COMPLETAS ===");
        Debug.Log($"Tiempo jugado: {timePlayed:F1}s");
        Debug.Log($"Precisión: {GetAccuracyPercentage():F1}% ({projectilesHit}/{totalProjectilesThrown})");
        Debug.Log($"Eficiencia pociones: {GetPotionEfficiencyPercentage():F1}%");
        Debug.Log($"Muertes jugador: {playerDeathsCount}");
        Debug.Log($"Enemigos eliminados: {GetTotalEnemiesKilled()}");
        Debug.Log($"KD Ratio: {GetKDRatio():F2}");
        Debug.Log("================================");
    } 

    #endregion

    #region Sistema de Guardado/Carga (OPCIONAL pero recomendado)
    private void LoadSavedData() {
        // Tiempo
        timePlayed = PlayerPrefs.GetFloat("TotalTimePlayed", 0f);

        // Accuracy
        totalProjectilesThrown = PlayerPrefs.GetInt("TotalProjectilesThrown", 0);
        projectilesHit = PlayerPrefs.GetInt("ProjectilesHit", 0);
        projectilesMissed = PlayerPrefs.GetInt("ProjectilesMissed", 0);

        // Pociones
        hpPotionsUsed = PlayerPrefs.GetInt("HpPotionsUsed", 0);
        manaPotionsUsed = PlayerPrefs.GetInt("ManaPotionsUsed", 0);
        totalPotionPointsConsumed = PlayerPrefs.GetInt("PotionPointsConsumed", 0);
        totalPotionPointsWasted = PlayerPrefs.GetInt("PotionPointsWasted", 0);

        // Muertes del jugador
        playerDeathsCount = PlayerPrefs.GetInt("PlayerDeaths", 0);
        killedByMageCount = PlayerPrefs.GetInt("KilledByMage", 0);
        killedByRangerCount = PlayerPrefs.GetInt("KilledByRanger", 0);
        killedByTankCount = PlayerPrefs.GetInt("KilledByTank", 0);

        // Enemigos eliminados
        mageEnemiesKilled = PlayerPrefs.GetInt("MageKills", 0);
        rangerEnemiesKilled = PlayerPrefs.GetInt("RangerKills", 0);
        tankEnemiesKilled = PlayerPrefs.GetInt("TankKills", 0);

        Debug.Log("DataMiner: Datos cargados");
    }

    private void SaveAllData() {
        // Tiempo
        PlayerPrefs.SetFloat("TotalTimePlayed", timePlayed);

        // Accuracy
        PlayerPrefs.SetInt("TotalProjectilesThrown", totalProjectilesThrown);
        PlayerPrefs.SetInt("ProjectilesHit", projectilesHit);
        PlayerPrefs.SetInt("ProjectilesMissed", projectilesMissed);

        // Pociones
        PlayerPrefs.SetInt("HpPotionsUsed", hpPotionsUsed);
        PlayerPrefs.SetInt("ManaPotionsUsed", manaPotionsUsed);
        PlayerPrefs.SetInt("PotionPointsConsumed", totalPotionPointsConsumed);
        PlayerPrefs.SetInt("PotionPointsWasted", totalPotionPointsWasted);

        PlayerPrefs.Save();
        Debug.Log("DataMiner: Todos los datos guardados");
    }

    public void ResetAllData() {
        // Resetear todas las variables
        playerDeathsCount = 0;
        mageEnemiesKilled = 0;
        rangerEnemiesKilled = 0;
        tankEnemiesKilled = 0;
        killedByMageCount = 0;
        killedByRangerCount = 0;
        killedByTankCount = 0;

        totalPotionPointsConsumed = 0;
        totalPotionPointsWasted = 0;
        hpPotionsUsed = 0;
        manaPotionsUsed = 0;

        totalProjectilesThrown = 0;
        projectilesMissed = 0;
        projectilesHit = 0;

        timePlayed = 0f;

        // Borrar PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("DataMiner: Todos los datos reseteados");
    }

    #endregion
}
