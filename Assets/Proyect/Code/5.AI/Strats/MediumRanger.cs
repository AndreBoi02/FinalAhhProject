
public class MediumRanger : IRangerLevels {
    public void Execute(Ranger ranger) {
        if (ranger.IsPlayerInSideRadius()) {
            ranger.MoveAwayFromPlayer();
        }
        else {
            ranger.StayInPlace();
            ranger.Attack();
        }
    }
}
