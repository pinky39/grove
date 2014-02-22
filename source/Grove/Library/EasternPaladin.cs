namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class EasternPaladin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Eastern Paladin")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Zombie Knight")
        .Text("{B}{B},{T} : Destroy target green creature.")
        .FlavorText(
          "Their fragile world. Their futile lives. They obstruct the Grand Evolution. In Yawgmoth's name, we shall excise them.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{B}{B},{T}: Destroy target green creature.";
            p.Cost = new AggregateCost(new PayMana("{B}{B}".Parse(), ManaUsage.Abilities), new Tap());
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Creature && c.HasColor(CardColor.Green))
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy, EffectTag.CreaturesOnly));
          });
    }
  }
}