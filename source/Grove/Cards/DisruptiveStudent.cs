namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class DisruptiveStudent : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Disruptive Student")
        .ManaCost("{2}{U}")
        .Type("Creature Human Wizard")
        .Text("{T}: Counter target spell unless its controller pays {1}.")
        .FlavorText(
          "'Teferi is a problem student. Always late for class. No appreciation for constructive use of time.'{EOL}—Barrin, progress report")
        .Power(1)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{T}: Counter target spell unless its controller pays {1}.",
            Cost<Tap>(),
            Effect<CounterTargetSpell>(e => e.DoNotCounterCost = 1.Colorless()),
            effectTarget: Target(
              Validators.CounterableSpell(),
              Zones.Stack()),
            targetingAi: TargetingAi.CounterSpell(),
            timing: Timings.CounterSpell(1)
            )
        );
    }
  }
}