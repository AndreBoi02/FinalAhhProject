
public class HardMage : IMageLevels {

    public void Execute(Mage mage) {
        if(mage.GetIsDead()) return;
        if(mage.GetStatHandler().Health < 20) {
            mage.HealAllies();
        }
        if (mage.IsPlayerInSideRadius()) {
            mage.TpAwayFromPlayer();
        }
        else {
            mage.Attack();
        }
    }
}
