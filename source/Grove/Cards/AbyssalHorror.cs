namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
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
        .Power(2)
        .Toughness(2)
        .Abilities(
          StaticAbility.Flying,
          C.TriggeredAbility(
            "When Abyssal Horror enters the battlefield, target player discards two cards.",
            C.Trigger<ChangeZone>((t, _) => t.To = Zone.Battlefield),
            C.Effect<TargetPlayerDiscardsCards>((e, _) => e.SelectedCount = 2),
            C.Selector(
              validator: target => target.IsPlayer(),
              scorer: TargetScores.OpponentOnly())));
    }
  }
}