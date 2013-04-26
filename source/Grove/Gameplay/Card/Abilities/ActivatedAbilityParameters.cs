namespace Grove.Gameplay.Card.Abilities
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