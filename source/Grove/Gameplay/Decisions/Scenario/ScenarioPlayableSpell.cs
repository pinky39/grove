namespace Grove.Core.Decisions.Scenario
{
  using System;
  using System.Linq;
  using Mana;
  using Results;

  public class ScenarioPlayableSpell : PlayableSpell
  {
    public override bool CanPlay()
    {
      if (Card == null)
        throw new InvalidOperationException("Did you forget to add card to players hand?");

      var manaCost = Card.GetSpellManaCost(Index);

      if (ActivationParameters.X.HasValue)
        manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());

      Controller.AddManaToManaPool(manaCost);
      var prerequisites = Card.CanCast();

      return prerequisites.Any(x => x.Index == Index);
    }
  }
}