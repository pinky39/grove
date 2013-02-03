namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;

  public class AlbinoTroll : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Albino Troll")
        .ManaCost("{1}{G}")
        .Type("Creature Troll")
        .Text(
          "{Echo} {1}{G} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}{1}{G}: Regenerate Albino Troll.")
        .Power(3)
        .Toughness(3)
        .Echo("{1}{G}")
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{G}: Regenerate Albino Troll.";
            p.Cost = new PayMana("{1}{G}".ParseMana(), ManaUsage.Abilities);
            p.Effect = () => new Regenerate {Category = EffectCategories.Protector};
            p.TimingRule(new Core.Ai.TimingRules.Regenerate());
          });
    }
  }
}