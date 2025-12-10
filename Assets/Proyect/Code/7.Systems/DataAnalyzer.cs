using UnityEngine;
using System.Collections.Generic;

public class DataAnalyzer : MonoBehaviour {
    // Referencia al DataMiner
    [SerializeField] private DataMiner dataMiner;

    // Perfiles del jugador
    public enum PlayerProfile {
        Novice,      // Principiante
        Intermediate, // Intermedio
        Advanced,     // Avanzado
        Expert        // Experto (opcional)
    }

    // Ponderaciones para cada métrica (ajustables)
    [Header("Weight Settings")]
    [SerializeField] private float accuracyWeight = 0.30f;      // 30%
    [SerializeField] private float efficiencyWeight = 0.25f;    // 25%
    [SerializeField] private float survivalWeight = 0.20f;      // 20%
    [SerializeField] private float combatWeight = 0.25f;        // 25%

    [Header("Profile Thresholds")]
    [SerializeField] private float noviceThreshold = 40f;       // < 40%
    [SerializeField] private float intermediateThreshold = 65f; // 40-65%
    [SerializeField] private float advancedThreshold = 85f;     // 65-85%

    // Historial de perfiles
    private List<PlayerProfile> profileHistory = new List<PlayerProfile>();
    private PlayerProfile currentProfile = PlayerProfile.Novice;

    // Evento para cuando cambia el perfil
    public struct ProfileChangedEvent : IEvent {
        public PlayerProfile NewProfile;
        public PlayerProfile OldProfile;
        public float OverallScore;

        public ProfileChangedEvent(PlayerProfile newProfile, PlayerProfile oldProfile, float score) {
            NewProfile = newProfile;
            OldProfile = oldProfile;
            OverallScore = score;
        }
    }

    private void Start() {
        if (dataMiner == null) {
            dataMiner = GetComponent<DataMiner>();
        }

        if (dataMiner == null) {
            Debug.LogError("DataAnalyzer: No se encontró DataMiner en la escena!");
            return;
        }

        // Analizar cada cierto tiempo o por evento
        InvokeRepeating(nameof(AnalyzePlayerPerformance), 30f, 30f); // Cada 30 segundos
    }

    public void AnalyzePlayerPerformance() {
        if (dataMiner == null) return;

        // Calcular puntuaciones individuales
        float accuracyScore = CalculateAccuracyScore();
        float efficiencyScore = CalculateEfficiencyScore();
        float survivalScore = CalculateSurvivalScore();
        float combatScore = CalculateCombatScore();

        // Calcular puntuación total ponderada
        float overallScore = (accuracyScore * accuracyWeight) +
                           (efficiencyScore * efficiencyWeight) +
                           (survivalScore * survivalWeight) +
                           (combatScore * combatWeight);

        // Determinar nuevo perfil
        PlayerProfile newProfile = DetermineProfile(overallScore);

        // Si cambió el perfil, notificar
        if (newProfile != currentProfile) {
            PlayerProfile oldProfile = currentProfile;
            currentProfile = newProfile;
            profileHistory.Add(newProfile);

            // Disparar evento
            EventBus<ProfileChangedEvent>.Raise(new ProfileChangedEvent(
                newProfile, oldProfile, overallScore
            ));

            Debug.Log($"PERFIL CAMBIADO: {oldProfile} - {newProfile} (Score: {overallScore:F1}%)");

            // Desbloquear contenido basado en perfil
            UnlockContentBasedOnProfile(newProfile);
        }

        // Mostrar análisis
        DisplayAnalysis(accuracyScore, efficiencyScore, survivalScore, combatScore, overallScore);
    }

    #region Métricas de Cálculo

    private float CalculateAccuracyScore() {
        // Precisión: 0-100% directamente
        return dataMiner.GetAccuracyPercentage();
    }

    private float CalculateEfficiencyScore() {
        // Eficiencia de pociones: 0-100%
        // 100% = no desperdició nada
        float efficiency = dataMiner.GetPotionEfficiencyPercentage();

        // Bonus por usar pociones estratégicamente
        int totalPotionsUsed = dataMiner.GetTotalPotionsUsed();
        if (totalPotionsUsed > 10) efficiency += 5f; // Experiencia
        if (totalPotionsUsed > 20) efficiency += 5f; // Muy experimentado

        return Mathf.Clamp(efficiency, 0f, 100f);
    }

    private float CalculateSurvivalScore() {
        // Basado en muertes vs tiempo jugado
        int playerDeaths = dataMiner.GetTotalPlayerDeaths();
        float timePlayed = dataMiner.GetTimePlayed();

        if (timePlayed < 60f) return 50f; // Primera hora, puntuación base

        // ERROR: Si playerDeaths es 0, deathsPerMinute sería 0
        // Lo correcto sería:
        if (playerDeaths == 0) return 100f; // ¡Perfecto! No murió

        // Puntuación = 100 - (muertes por minuto * factor)
        float deathsPerMinute = playerDeaths / (timePlayed / 60f);
        float survivalScore = 100f - (deathsPerMinute * 20f);

        return Mathf.Clamp(survivalScore, 0f, 100f);
    }

    private float CalculateCombatScore() {
        // Basado en eficiencia en combate
        int enemiesKilled = dataMiner.GetTotalEnemiesKilled();
        int playerDeaths = dataMiner.GetTotalPlayerDeaths();

        if (playerDeaths == 0) return 100f; // Perfecto!

        // KD Ratio ajustado
        float kdRatio = (float)enemiesKilled / playerDeaths;
        float combatScore = Mathf.Min(kdRatio * 25f, 100f); // KD de 4 = 100%

        // Bonus por variedad
        var killsByType = dataMiner.GetEnemiesKilledByType();
        int differentTypes = 0;
        if (killsByType.mage > 0) differentTypes++;
        if (killsByType.ranger > 0) differentTypes++;
        if (killsByType.tank > 0) differentTypes++;

        combatScore += differentTypes * 5f; // +5% por cada tipo

        return Mathf.Clamp(combatScore, 0f, 100f);
    }

    #endregion

    private PlayerProfile DetermineProfile(float overallScore) {
        if (overallScore >= advancedThreshold) return PlayerProfile.Advanced;
        if (overallScore >= intermediateThreshold) return PlayerProfile.Intermediate;
        return PlayerProfile.Novice;
    }

    private void UnlockContentBasedOnProfile(PlayerProfile profile) {
        switch (profile) {
            case PlayerProfile.Intermediate:
                Debug.Log("DESBLOQUEADO: Nuevos enemigos tipo 'Ranger'");
                // Ejemplo: EnemySpawner.Instance.UnlockEnemyType(EnemyType.Ranger);
                break;

            case PlayerProfile.Advanced:
                Debug.Log("DESBLOQUEADO: Enemigos élite y jefes");
                Debug.Log("DESBLOQUEADO: Arma especial 'Espada de Élite'");
                // EnemySpawner.Instance.UnlockEnemyType(EnemyType.Elite);
                // WeaponManager.Instance.UnlockWeapon("EliteSword");
                break;

            case PlayerProfile.Expert:
                Debug.Log("DESBLOQUEADO: Modo desafío extremo");
                // GameManager.Instance.UnlockGameMode("ExtremeChallenge");
                break;
        }
    }

    private void DisplayAnalysis(float accuracy, float efficiency, float survival, float combat, float overall) {
        Debug.Log("=== ANÁLISIS DEL JUGADOR ===");
        Debug.Log($"Precisión: {accuracy:F1}%");
        Debug.Log($"Eficiencia: {efficiency:F1}%");
        Debug.Log($"Supervivencia: {survival:F1}%");
        Debug.Log($"Combate: {combat:F1}%");
        Debug.Log($"Puntuación Total: {overall:F1}%");
        Debug.Log($"Perfil Actual: {currentProfile}");
        Debug.Log("=============================");
    }

    #region Métodos Públicos para UI

    public PlayerProfile GetCurrentProfile() => currentProfile;

    public float GetOverallScore() {
        float accuracy = CalculateAccuracyScore();
        float efficiency = CalculateEfficiencyScore();
        float survival = CalculateSurvivalScore();
        float combat = CalculateCombatScore();

        return (accuracy * accuracyWeight) +
               (efficiency * efficiencyWeight) +
               (survival * survivalWeight) +
               (combat * combatWeight);
    }

    public (float accuracy, float efficiency, float survival, float combat) GetIndividualScores() {
        return (
            CalculateAccuracyScore(),
            CalculateEfficiencyScore(),
            CalculateSurvivalScore(),
            CalculateCombatScore()
        );
    }

    public string GetProfileDescription(PlayerProfile profile) {
        switch (profile) {
            case PlayerProfile.Novice:
                return "Principiante - Sigue practicando!";
            case PlayerProfile.Intermediate:
                return "Intermedio - Buen desempeño";
            case PlayerProfile.Advanced:
                return "Avanzado - Jugador experimentado";
            case PlayerProfile.Expert:
                return "Experto - Maestro del juego";
            default:
                return "Desconocido";
        }
    }

    #endregion
}
