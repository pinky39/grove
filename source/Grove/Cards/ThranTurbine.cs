namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class ThranTurbine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Thran Turbine")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.")
        .FlavorText("When Urza asked the viashino what it did, they answered: 'It hums.'")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.",
            C.Trigger<AtBegginingOfStep>(t =>
              {
                t.Step = Step.Upkeep;
                t.Order = TriggerOrder.Last;
              }),
            C.Effect<AddManaToPool>(e =>
              {
                e.Amount = 2.AsColorlessMana();
                e.UseOnlyForAbilities = true;
              })
            )
        );
    }
  }
}