namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class UnworthyDead : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Unworthy Dead")
        .ManaCost("{1}{B}")
        .Type("Creature Skeleton")
        .Text("{B}: Regenerate Unworthy Dead.")
        .FlavorText(
          "Great Yawgmoth moves across the seas of shard and bone and rust. We exalt him in life, in death, and in between.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{B}: Regenerate Unworthy Dead.";
            p.Cost = new PayMana(Mana.Black, ManaUsage.Abilities);
            p.Effect = () => new Regenerate();
            p.TimingRule(new Core.Ai.TimingRules.Regenerate());
          });
    }
  }
}