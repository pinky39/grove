namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.CombatRules;
  using Artifical.TargetingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class JhoirasToolbox : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jhoira's Toolbox")
        .ManaCost("{2}")
        .Type("Artifact Creature Insect")
        .Text("{2}: Regenerate target artifact creature.")
        .FlavorText("It entertained Jhoira to craft a kit that could bring her the tools.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Regenerate target artifact creature.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities);
            p.Effect = () => new RegenerateTarget();
            p.TargetSelector.AddEffect(trg => 
              trg.Is.Card(c => c.Is().Creature && c.Is().Artifact).On.Battlefield());

            p.TargetingRule(new GainRegenerate());
          })
        .CombatRule(() => new Regenerate(2.Colorless()));
    }
  }
}