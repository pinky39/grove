namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

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

            p.TimingRule(new WhenCardHas(c => c.IsTapped));
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}