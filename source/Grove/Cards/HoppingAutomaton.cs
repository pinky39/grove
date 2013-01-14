namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Mana;
  using Core.Modifiers;

  public class HoppingAutomaton : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hopping Automaton")
        .ManaCost("{3}")
        .Type("Artifact Creature")
        .Text(
          "{0}: Hopping Automaton gets -1/-1 and gains flying until end of turn.")
       .Power(2) 
       .Toughness(2)
       .Abilities(
          ActivatedAbility(
            "{0}: Hopping Automaton gets -1/-1 and gains flying until end of turn.",
            Cost<PayMana>(c => c.Amount = ManaAmount.Zero),
            Effect<Core.Effects.ApplyModifiersToSelf>(e =>
              {
                e.ToughnessReduction = 1;
                e.Modifiers(Modifier<AddPowerAndToughness>(m =>
                  {
                    m.Power = -1;
                    m.Toughness = -1;
                  }),
                  Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Flying));
              }),
            timing: All(Timings.Has(x => x.Toughness > 1), Timings.Steps(Step.BeginningOfCombat)))
        );
    }
  }
}