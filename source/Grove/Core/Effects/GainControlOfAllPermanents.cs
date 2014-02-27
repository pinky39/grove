namespace Grove.Effects
{
  using System;
  using System.Linq;
  using Modifiers;

  public class GainControlOfAllPermanents : Effect
  {
    private readonly Func<Card, Effect, bool> _filter;

    private GainControlOfAllPermanents() {}

    public GainControlOfAllPermanents(Func<Card, Effect, bool> filter)
    {
      _filter = filter;
    }

    protected override void ResolveEffect()
    {
      var permanents = Players.Permanents()
        .Where(x => _filter(x, this))
        .ToList();

      foreach (var permanent in permanents)
      {
        if (permanent.Controller != Controller)
        {
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,
              X = X
            };

          var modifier = new ChangeController(Controller);
          permanent.AddModifier(modifier, p);
        }
      }
    }
  }
}