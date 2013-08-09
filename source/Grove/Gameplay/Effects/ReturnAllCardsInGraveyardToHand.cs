namespace Grove.Gameplay.Effects
{
  using System;
  using System.Linq;

  public class ReturnAllCardsInGraveyardToHand : Effect
  {
    private readonly Func<Card, Game, bool> _filter;

    private ReturnAllCardsInGraveyardToHand() {}

    public ReturnAllCardsInGraveyardToHand(Func<Card, bool> filter) : this((c, g) => filter(c)) {}

    public ReturnAllCardsInGraveyardToHand(Func<Card, Game, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      foreach (var permanent in Controller.Graveyard.Where(card => _filter(card, Game)).ToList())
      {
        permanent.PutToHand();
      }
    }
  }
}