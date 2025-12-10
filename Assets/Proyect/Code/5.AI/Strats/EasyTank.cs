
public class EasyTank : ITankLevels {
    public void Execute(Tank tank) {
        
        if (tank.IsPlayerInSideRadius()) {
            tank.StayInPlace();
            tank.Attack();
        }
        else {
            tank.GoForThePlayerSeek();
        }
    }
}
