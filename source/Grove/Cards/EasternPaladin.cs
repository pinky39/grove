namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Characteristics;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

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

            p.TargetingRule(new Destroy());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}