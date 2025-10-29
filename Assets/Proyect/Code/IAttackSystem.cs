public interface IAttackSystem {

    public event System.Action OnPrepareAttack;
    event System.Action OnAttack;
    event System.Action OnNextWeapon;
    event System.Action OnPrevWeapon;

    AttackSystem AttackSystem { get; }
}
