namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.Triggers;

  public class Retromancer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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

            p.Trigger(new OnBeingTargetedBySpellOrAbility());

            p.Effect = () => new DealDamageToPlayer(
              amount: 3,
              player: P(e => e.TriggerMessage<EffectPutOnStackEvent>().Effect.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}