namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Triggers;
  using Core.Zones;

  public class AbyssalHorror : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Abyssal Horror")
        .ManaCost("{4}{B}{B}")
        .Type("Creature - Specter")
        .Text("{Flying}{EOL}When Abyssal Horror enters the battlefield, target player discards two cards.")
        .FlavorText("'It has no face of its own—it wears that of its latest victim.'")        
        .Power(2)
        .Toughness(2)
        .Abilities(
          Static.Flying,
          TriggeredAbility(
            "When Abyssal Horror enters the battlefield, target player discards two cards.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<Core.Effects.DiscardCards>(p => p.Effect.Count = 2),
            Target(Validators.Player(), Zones.None()), 
            TargetingAi.Opponent()));
    }
  }
}