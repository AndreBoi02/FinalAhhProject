
public class EasyMage : IMageLevels {

    public void Execute(Mage mage) {
        if (mage.GetIsDead()) return;
        mage.Attack();
    }
}
