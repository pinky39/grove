namespace Grove
{
    using System;
    using Costs;

  public class ActivatedAbilityParameters : AbilityParameters
  {
    public bool ActivateAsSorcery;
    public bool ActivateOnlyOnceEachTurn;
    public Zone ActivationZone = Zone.Battlefield;
    public Cost Cost;
    public Action<Card> PutToZoneAfterActivation = delegate { };
  }
}