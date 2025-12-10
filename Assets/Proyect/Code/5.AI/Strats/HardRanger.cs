using UnityEngine;

public class HardRanger : IRangerLevels {
    public void Execute(Ranger ranger) {
        if (ranger.DistanceFromPlayer() > 10) {
            Debug.Log("Predicted Attack");
            ranger.StayInPlace();
            ranger.PredictPlayerPos();
            ranger.Attack();
        }
        else if(!ranger.IsPlayerInSideRadius() && (ranger.DistanceFromPlayer() <= 10)) {
            Debug.Log("Normal Attack");
            ranger.StayInPlace();
            ranger.Attack();
        }
        else {
            ranger.MoveAwayFromPlayer();
        }
    }
}
