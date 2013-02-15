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

  public class LoomingShade : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Looming Shade")
        .ManaCost("{2}{B}")
        .Type("Creature - Shade")
        .Text("{B}: Looming Shade gets +1/+1 until end of turn.")
        .FlavorText(
          "The shade tracks victims by reverberations of the pipes, as a spider senses prey tangled in its trembling web.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{B}: Looming Shade gets +1/+1 until end of turn.";
            p.Cost = new PayMana(ManaAmount.Black, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(1, 1) {UntilEot = true})
                  {Category = EffectCategories.ToughnessIncrease};
            p.TimingRule(new IncreaseOwnersPowerOrToughness(1, 1));
          });
    }
  }
}