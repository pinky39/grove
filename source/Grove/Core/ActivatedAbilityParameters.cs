namespace Grove.Core
{
  using Costs;
  using Zones;

  public class ActivatedAbilityParameters : AbilityParameters
  {
    public bool ActivateAsSorcery;
    public Zone ActivationZone = Zone.Battlefield;
    public Cost Cost;
  }
}