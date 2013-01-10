namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class HollowDogs : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hollow Dogs")
        .ManaCost("{4}{B}")
        .Type("Creature Zombie Hound")
        .Text("Whenever Hollow Dogs attacks, it gets +2/+0 until end of turn.")
        .FlavorText(
          "A hollow dog is never empty. It is filled with thirst for the hunt.")
        .Power(2)
        .Toughness(2)
        .Abilities(
          TriggeredAbility(
            "Whenever Hollow Dogs attacks, it gets +2/+0 until end of turn.",
            Trigger<OnAttack>(),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;                
                }, untilEndOfTurn: true))),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}