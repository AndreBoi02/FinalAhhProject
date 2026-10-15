using UnityEngine;

[RequireComponent(typeof(StatHandler))]
public class PotionHandler : MonoBehaviour {
    private StatHandler statHandler;

    private void Awake() {
        statHandler = GetComponent<StatHandler>();
    }

    public void UseHpPot() {
        if (statHandler.HpPotAvailable() && statHandler.Health != 100) {
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
        }
    }

    public void UseManaPot() {
        if (statHandler.ManaPotAvailable() && statHandler.Mana != 50) {
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
        }
    }
}
