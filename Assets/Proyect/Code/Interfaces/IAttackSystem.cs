public interface IAttackSystem {
    event System.Action OnPrepareAttack;
    event System.Action OnAttack;
    

    AttackSystem AttackSystem { get; }
}
