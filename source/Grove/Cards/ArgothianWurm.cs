namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class ArgothianWurm : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Argothian Wurm")
        .ManaCost("{3}{G}")
        .Type("Creature - Wurm")
        .Text(
          "{Trample}{EOL}When Argothian Wurm enters the battlefield, any player may sacrifice a land. If a player does, put Argothian Wurm on top of its owner's library.")
        .Power(6)
        .Toughness(6)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Trample,
          C.TriggeredAbility(
            "When Argothian Wurm enters the battlefield, any player may sacrifice a land. If a player does, put Argothian Wurm on top of its owner's library.",
            C.Trigger<ChangeZone>(t => t.To = Zone.Battlefield),
            C.Effect<PutOnTopOfLibraryUnlessOpponentSacLand>()
            ));
    }
  }
}