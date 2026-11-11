using UnityEngine;

[RequireComponent(typeof(StatHandler))]
public class PotionHandler : MonoBehaviour {
    [SerializeField] private float potionCooldown = 0.25f;
    private float lastPotionTime = -999f;

    private StatHandler statHandler;

    private void Awake() {
        statHandler = GetComponent<StatHandler>();
    }

    public void UseHpPot() {
        if (Time.time < lastPotionTime + potionCooldown) return;

        if (statHandler.HpPotAvailable() && statHandler.Health != 100) {
            lastPotionTime = Time.time;
            statHandler.HpPot -= 1;
            int wastedPoints = 0;
            int consumedPoints = 20; // Valor de la poción

            if (statHandler.Health >= 81) {
                // Calcular desperdicio
                wastedPoints = ((int)statHandler.Health + 20) - 100;
                statHandler.Health = 100;

                // Puntos consumidos realmente
                consumedPoints = 20 - wastedPoints;
                Debug.Log($"Pocion desperdiciada: {wastedPoints} puntos");
            }
            else {
                statHandler.Health += 20;
            }

            // Disparar evento para DataMiner / Telemetría
            PotionEfficiencyEvent potionEvent = new PotionEfficiencyEvent(
                gameObject,
                consumedPoints,
                wastedPoints,
                "HP"
            );
            EventBus<PotionEfficiencyEvent>.Raise(potionEvent);
        }
    }

    public void UseManaPot() {
        if (Time.time < lastPotionTime + potionCooldown) return;

        if (statHandler.ManaPotAvailable() && statHandler.Mana != 50) {
            lastPotionTime = Time.time;
            statHandler.ManaPot -= 1;
            int wastedPoints = 0;
            int consumedPoints = 15; // Valor de la poción

            if (statHandler.Mana >= 36) {
                wastedPoints = ((int)statHandler.Mana + 15) - 50;
                statHandler.Mana = 50;

                // Puntos consumidos realmente
                consumedPoints = 15 - wastedPoints;
                Debug.Log($"Mana pocion desperdiciada: {wastedPoints} puntos");
            }
            else {
                statHandler.Mana += 15;
            }

            // Disparar evento para DataMiner / Telemetría
            PotionEfficiencyEvent potionEvent = new PotionEfficiencyEvent(
                gameObject,
                consumedPoints,
                wastedPoints,
                "Mana"
            );
            EventBus<PotionEfficiencyEvent>.Raise(potionEvent);
        }
    }
}
