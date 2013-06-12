﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.CostRules;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class StrokeOfGenius : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Stroke of Genius")
        .ManaCost("{2}{U}").HasXInCost()
        .Type("Instant")
        .Text("Target player draws X cards.")
        .FlavorText(
          "After a hundred failed experiments, Urza was stunned to find that common silver passed through the portal undamaged. He immediately designed a golem made of the metal.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerDrawsCards(cardCount: Value.PlusX);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());

            p.TimingRule(new EndOfTurn());
            p.TimingRule(new ControllerHasMana(6));
            p.TargetingRule(new SpellOwner());
            p.CostRule(new MaxAvailableMana());
          });
    }
  }
}