using UnityEngine;

public class HardTank : ITankLevels {
    public void Execute(Tank tank) {
        if (tank.GetSheltered().GetIsDead()) {
            if (tank.IsPlayerInSideRadius()) {
                tank.StayInPlace();
                tank.Attack();
            }
            else {
                tank.PredictPlayer();
            }
        }
        else {
            tank.ProtectAgent();
        }
    }
}
