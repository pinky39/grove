namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class DisruptiveStudent : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Disruptive Student")
        .ManaCost("{2}{U}")
        .Type("Creature Human Wizard")
        .Text("{T}: Counter target spell unless its controller pays {1}.")
        .FlavorText(
          "'Teferi is a problem student. Always late for class. No appreciation for constructive use of time.'{EOL}—Barrin, progress report")
        .Power(1)
        .Toughness(1)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{T}: Counter target spell unless its controller pays {1}.",
            C.Cost<TapOwnerPayMana>(cost => cost.TapOwner = true),
            C.Effect<CounterTargetSpell>(e => e.DoNotCounterCost = 1.AsColorlessMana()),
            effectValidator: C.Validator(Validators.Counterspell()),
            selectorAi: TargetSelectorAi.CounterSpell(),
            timing: Timings.CounterSpell(1)
            )
        );
    }
  }
}