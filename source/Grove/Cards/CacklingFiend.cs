namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class CacklingFiend : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Cackling Fiend")
        .ManaCost("{2}{B}{B}")
        .Type("Creature - Zombie")
        .Text("When Cackling Fiend enters the battlefield, each opponent discards a card.")
        .FlavorText("Its windpipe is only the first to amplify its maddening laughter.")
        .Power(2)
        .Toughness(1)
        .Timing(Timings.FirstMain())
        .Abilities(
          C.TriggeredAbility(
            "When Cackling Fiend enters the battlefield, each opponent discards a card.",
            C.Trigger<OnZoneChange>((t, _) => t.To = Zone.Battlefield),
            C.Effect<OpponentDiscardsCards>(e => e.SelectedCount = 1)));
    }
  }
}