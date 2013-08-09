namespace Grove.Gameplay.Abilities
{
  public interface IStaticAbilities
  {
    bool Deathtouch { get; }
    bool Defender { get; }
    bool Fear { get; }
    bool Flying { get; }
    bool Haste { get; }
    bool Hexproof { get; }
    bool Indestructible { get; }
    bool Lifelink { get; }
    bool Shroud { get; }
    bool Trample { get; }
    bool Unblockable { get; }
    bool FirstStrike { get; }
    bool DoubleStrike { get; }
    bool Reach { get; }
    bool Vigilance { get; }
    bool Swampwalk { get; }
    bool CannotAttack { get; }
    bool CannotBlock { get; }
    bool Islandwalk { get; }
    bool Mountainwalk { get; }
    bool DoesNotUntap { get; }
    bool AssignsDamageAsThoughItWasntBlocked { get; }
    bool AnyEvadingAbility { get; }
    bool CanOnlyBeBlockedByCreaturesWithFlying { get; }
    bool CanAttackOnlyIfDefenderHasIslands { get; }
    bool UnblockableIfDedenderHasArtifacts { get; }
    bool Flash { get; }
    bool AttacksEachTurnIfAble { get; }
    bool Has(Static ability);
  }
}