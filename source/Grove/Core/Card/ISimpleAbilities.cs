namespace Grove
{
  public interface ISimpleAbilities
  {
    bool Convoke { get; }
    bool Deathtouch { get; }
    bool Defender { get; }
    bool Delve { get; }
    bool Fear { get; }
    bool Flying { get; }
    bool Haste { get; }
    bool Hexproof { get; }
    bool Indestructible { get; }
    bool Intimidate { get; }
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
    bool CanOnlyBeBlockedByWalls { get; }
    bool CanBlockOnlyCreaturesWithFlying { get; }
    bool CannotBeBlockedByWalls { get; }
    bool CanAttackOnlyIfDefenderHasIslands { get; }
    bool UnblockableIfDedenderHasArtifacts { get; }
    bool UnblockableIfDedenderHasEnchantments { get; }
    bool Flash { get; }
    bool AttacksEachTurnIfAble { get; }
    bool Forestwalk { get; }
    bool Lure { get; }
    bool Echo { get; }
    bool Has(Static ability);
  }
}