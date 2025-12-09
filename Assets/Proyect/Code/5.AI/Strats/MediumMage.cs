
public class MediumMage : IMageLevels {
    public void Execute(Mage mage) {
        if (mage.IsPlayerInSideRadius()) {
            mage.TpAwayFromPlayer();
        }
        else {
            mage.Attack();
        }
    }
}
