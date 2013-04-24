namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class HoppingAutomaton : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hopping Automaton")
        .ManaCost("{3}")
        .Type("Artifact Creature")
        .Text(
          "{0}: Hopping Automaton gets -1/-1 and gains flying until end of turn.")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{0}: Hopping Automaton gets -1/-1 and gains flying until end of turn.";
            p.Cost = new PayMana(Mana.Zero, ManaUsage.Abilities);
            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(-1, -1) {UntilEot = true},
              () => new AddStaticAbility(Static.Flying) {UntilEot = true}) {ToughnessReduction = 1};

            p.TimingRule(new Steps(Step.BeginningOfCombat));            
            p.TimingRule(new OwningCardHas(c => c.Toughness > 1 && !c.Has().Flying));
          });
    }
  }
}