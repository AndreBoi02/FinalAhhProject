public interface IAttackSystem {

    event System.Action OnPrepareAttack;
    event System.Action OnAttack;
    event System.Action OnNextWeapon;
    event System.Action OnPrevWeapon;

    AttackSystem AttackSystem { get; }
}
