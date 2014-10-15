namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class SelflessCathar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Selfless Cathar")
        .ManaCost("{W}")
        .Type("Creature — Human Cleric")
        .Text("{1}{W}, Sacrifice Selfless Cathar: Creatures you control get +1/+1 until end of turn.")
        .FlavorText("\"If I fail to offer myself, we will surely be overrun. My fate would be the same.\"")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{W}, Sacrifice Selfless Cathar: Creatures you control get +1/+1 until end of turn.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{W}".Parse(), ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new ApplyModifiersToPermanents((e, c) => c.Is().Creature, ControlledBy.SpellOwner,
              () => new AddPowerAndToughness(1, 1) {UntilEot = true});

            p.TimingRule(new Any(
              new AfterOpponentDeclaresAttackers(),
              new AfterOpponentDeclaresBlockers()));
          });
    }
  }
}