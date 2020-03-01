namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class MarduScout : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mardu Scout")
        .ManaCost("{R}{R}")
        .Type("Creature — Goblin Scout")
        .Text("Dash {1}{R}{I}(You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.){/I}")
        .FlavorText("The Mardu all enjoy war, but only the goblins make a game of it.")
        .Power(3)
        .Toughness(1)
        .Dash("{1}{R}");   
    }
  }
}
