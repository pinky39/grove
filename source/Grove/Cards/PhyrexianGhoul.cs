namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class PhyrexianGhoul : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Phyrexian Ghoul")
        .ManaCost("{2}{B}")
        .Type("Creature - Zombie")
        .Text("Sacrifice a creature: Phyrexian Ghoul gets +2/+2 until end of turn.")
        .FlavorText("Phyrexia wastes nothing. Its food chain is a spiraling cycle.")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice a creature: Phyrexian Ghoul gets +2/+2 until end of turn.";
            p.Cost = new Sacrifice();
            p.Effect = () => new ApplyModifiersToSelf(() => new AddPowerAndToughness(2, 2) {UntilEot = true})
              {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddCost(trg => trg.Is.Creature(ControlledBy.SpellOwner));
            p.TimingRule(new IncreaseOwnersPowerOrToughness(2, 2));
            p.TargetingRule(new OrderByRank(c => c.Score));
          });
    }
  }
}