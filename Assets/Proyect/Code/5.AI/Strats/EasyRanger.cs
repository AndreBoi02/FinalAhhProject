
public class EasyRanger : IRangerLevels {
    public void Execute(Ranger ranger) {
        if (ranger.GetIsDead()) return;
        ranger.Attack();
    }
}
