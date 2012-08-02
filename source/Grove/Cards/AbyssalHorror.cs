namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class AbyssalHorror : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Abyssal Horror")
        .ManaCost("{4}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Abyssal Horror enters the battlefield, target player discards two cards.")
        .FlavorText("'It has no face of its own—it wears that of its latest victim.'")
        .Timing(Timings.FirstMain())
        .Power(2)
        .Toughness(2)
        .Abilities(
          Static.Flying,
          C.TriggeredAbility(
            "When Abyssal Horror enters the battlefield, target player discards two cards.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<TargetPlayerDiscardsCards>(p => p.Effect.SelectedCount = 2),
            C.Validator(Validators.Player()),
            selectorAi: TargetSelectorAi.Opponent()));
    }
  }
}