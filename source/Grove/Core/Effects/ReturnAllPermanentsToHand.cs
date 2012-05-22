namespace Grove.Core.Effects
{
  using System;
  using System.Linq;
  using Zones;

  public class ReturnAllPermanentsToHand : Effect
  {
    public Func<Card, bool> Filter = (card) => true;
    
    public override void Resolve()
    {
      foreach (var permanent in Players.Permanents().Where(card => Filter(card)).ToList())
      {
        permanent.ReturnToHand();
      }
    }
  }
}