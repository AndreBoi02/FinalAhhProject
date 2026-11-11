public interface IWeapon {
    void Initialize(CharacterAnimationHandler animHandler);
    void Equip();
    void Unequip();
    void PrepareAttack();
    void ExecuteAttack();
}
