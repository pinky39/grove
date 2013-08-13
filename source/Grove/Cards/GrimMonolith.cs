namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;


  public class GrimMonolith : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Grim Monolith")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "Grim Monolith doesn't untap during your untap step.{EOL}{T}: Add {3} to your mana pool.{EOL}{4}: Untap Grim Monolith.")
        .FlavorText("Part prison, part home.")
        .SimpleAbilities(Static.DoesNotUntap)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {3} to your mana pool.";
            p.ManaAmount(3.Colorless());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{4}: Untap Grim Monolith.";
            p.Cost = new PayMana(4.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new OwningCardHas(c => c.IsTapped));
            p.TimingRule(new EndOfTurn());
          });
    }
  }
}