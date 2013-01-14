namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;

  public class ThranTurbine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Thran Turbine")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.")
        .FlavorText("When Urza asked the viashino what it did, they answered: 'It hums.'")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.",
            Trigger<OnStepStart>(t =>
              {
                t.Step = Step.Upkeep;
                t.Order = TriggerOrder.Last;
              }),
            Effect<AddManaToPool>(e =>
              {
                e.Amount = 2.Colorless();
                e.UseOnlyForAbilities = true;
              }), triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}