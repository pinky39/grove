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

  public class SlinkingSkirge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Slinking Skirge")
        .ManaCost("{3}{B}")
        .Type("Creature Imp")
        .Text("{Flying}{EOL}{2}, Sacrifice Slinking Skirge: Draw a card.")
        .FlavorText(
          "Davvol encouraged the skirges; they made excellent sentries and were quite edible if properly seasoned.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}, Sacrifice Slinking Skirge: Draw a card.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new DrawCards(1);

            p.TimingRule(new Any(
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));

            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}