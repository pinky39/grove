namespace Grove.Core.Decisions.Scenario
{
  using System;
  using Cards;
  using Mana;
  using Results;

  public class ScenarioSpell : Spell
  {
    public ScenarioSpell(Card card, ActivationParameters activationParameters) : base(card, activationParameters) {}

    private Player Controller { get { return Card.Controller; } }

    public override bool CanPlay()
    {
      IManaAmount manaCost = ManaAmount.Zero;

      if (Card == null)
        throw new InvalidOperationException("Did you forget to add card to players hand?");

      if (Card.ManaCost != null)
      {
        manaCost = manaCost.Add(Card.ManaCost);
      }

      if (Card.KickerCost != null)
      {
        manaCost = manaCost.Add(Card.KickerCost);
      }


      if (ActivationParameters.X.HasValue)
        manaCost = manaCost.Add(ActivationParameters.X.Value);

      Controller.AddManaToManaPool(manaCost);
      var prerequisites = Card.CanCast();
      return prerequisites.CanBeSatisfied;
    }
  }
}