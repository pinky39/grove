namespace Grove.Core.Effects
{
  using System;
  using System.Linq;

  public class ReturnAllPermanentsToHand : Effect
  {
    public Func<Card, bool> Filter = (card) => true;

    protected override void ResolveEffect()
    {
      foreach (var permanent in Core.Players.Permanents().Where(card => Filter(card)).ToList())
      {
        permanent.PutToHand();
      }
    }
  }
}