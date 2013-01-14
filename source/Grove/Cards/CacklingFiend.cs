namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class CacklingFiend : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Cackling Fiend")
        .ManaCost("{2}{B}{B}")
        .Type("Creature - Zombie")
        .Text("When Cackling Fiend enters the battlefield, each opponent discards a card.")
        .FlavorText("Its windpipe is only the first to amplify its maddening laughter.")
        .Power(2)
        .Toughness(1)
        .Cast(p => p.Timing = Timings.FirstMain())        
        .Abilities(
          TriggeredAbility(
            "When Cackling Fiend enters the battlefield, each opponent discards a card.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<OpponentDiscardsCards>(e => e.SelectedCount = 1)));
    }
  }
}