namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Zones;

  public class ArgothianWurm : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Argothian Wurm")
        .ManaCost("{3}{G}")
        .Type("Creature - Wurm")
        .Text(
          "{Trample}{EOL}When Argothian Wurm enters the battlefield, any player may sacrifice a land. If a player does, put Argothian Wurm on top of its owner's library.")
        .Power(6)
        .Toughness(6)
        .Abilities(
          Static.Trample,
          TriggeredAbility(
            "When Argothian Wurm enters the battlefield, any player may sacrifice a land. If a player does, put Argothian Wurm on top of its owner's library.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<PutOnTopOfLibraryUnlessOpponentSacsLand>()));
    }
  }
}