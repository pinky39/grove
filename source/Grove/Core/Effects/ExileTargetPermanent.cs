namespace Grove.Core.Effects
{
  public class ExileTargetPermanent : Effect
  {
    public bool ControllerGainsLifeEqualToToughness { get; set; }
    
    public override void Resolve()
    {
      if (ControllerGainsLifeEqualToToughness)
      {
        Target.Card().Controller.Life += Target.Card().Toughness.Value;
      }
      
      Target.Card().Exile();      
    }
  }
}