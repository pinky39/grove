namespace Grove.Gameplay.Abilities
{
  using Costs;
  using Zones;

  public class ActivatedAbilityParameters : AbilityParameters
  {
    public bool ActivateAsSorcery;
    public bool ActivateOnlyOnceEachTurn;
    public Zone ActivationZone = Zone.Battlefield;
    public Cost Cost;
  }
}