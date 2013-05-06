namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Messages;
  using Gameplay.Misc;
  using Gameplay.Triggers;

  public class Retromancer : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Retromancer")
        .ManaCost("{2}{R}{R}")
        .Type("Creature Viashino Shaman")
        .Text(
          "Whenever Retromancer becomes the target of a spell or ability, Retromancer deals 3 damage to that spell or ability's controller.")
        .FlavorText("If one harm us, strike them in return. So sayeth the bey.")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Retromancer becomes the target of a spell or ability, Retromancer deals 3 damage to that spell or ability's controller.";

            p.Trigger(new OnBeingTargetedBySpell());

            p.Effect = () => new DealDamageToPlayer(
              amount: 3,
              player: P(e => e.TriggerMessage<PlayerHasCastASpell>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}