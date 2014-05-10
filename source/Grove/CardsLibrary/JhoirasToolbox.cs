namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.CombatRules;
  using AI.TargetingRules;
  using Costs;
  using Effects;

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

            p.TargetingRule(new EffectGiveRegenerate());
          })
        .CombatRule(() => new RegenerateCombatRule(2.Colorless()));
    }
  }
}