
public class MediumMage : IMageLevels {
    public void Execute(Mage mage) {
        if (mage.GetIsDead()) return;
        if (mage.IsPlayerInSideRadius()) {
            mage.TpAwayFromPlayer();
        }
        else {
            mage.Attack();
        }
    }
}
