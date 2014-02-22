namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class WesternPaladin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Western Paladin")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Zombie Knight")
        .Text("{B}{B},{T} : Destroy target white creature.")
        .FlavorText(
          "Their weak laws. Their flawed systems. They inhibit the Grand Evolution. In Yawgmoth's name, we shall erase them.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{B}{B},{T}: Destroy target white creature.";
            p.Cost = new AggregateCost(new PayMana("{B}{B}".Parse(), ManaUsage.Abilities), new Tap());
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && c.HasColor(CardColor.White))
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy, EffectTag.CreaturesOnly));
          });
    }
  }
}