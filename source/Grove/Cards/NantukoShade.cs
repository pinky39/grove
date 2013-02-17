namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class NantukoShade : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Nantuko Shade")
        .ManaCost("{B}{B}")
        .Type("Creature - Insect Shade")
        .Text("{B}: Nantuko Shade gets +1/+1 until end of turn.")
        .FlavorText("In life, the nantuko study nature by revering it. In death, they study nature by disemboweling it.")
        .Power(2)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{B}: Nantuko Shade gets +1/+1 until end of turn.";
            p.Cost = new PayMana(ManaAmount.Black, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 1) {UntilEot = true})
              {Category = EffectCategories.ToughnessIncrease};
            p.TimingRule(new IncreaseOwnersPowerOrToughness(1, 1));
          });
    }
  }
}