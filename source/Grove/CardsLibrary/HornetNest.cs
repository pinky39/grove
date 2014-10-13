namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Events;
  using Triggers;

  public class HornetNest : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hornet Nest")
        .ManaCost("{2}{G}")
        .Type("Creature - Insect")
        .Text("{Defender} {I}(This creature can't attack.){/I}{EOL}Whenever Hornet Nest is dealt damage, put that many 1/1 green Insect creature tokens with flying and deathtouch onto the battlefield. {I}(Any amount of damage a creature with deathtouch deals to a creature is enough to destroy it.){/I}")
        .Power(0)
        .Toughness(2)
        .SimpleAbilities(Static.Defender)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Hornet Nest is dealt damage, put that many 1/1 green Insect creature tokens with flying and deathtouch onto the battlefield.";

          p.Trigger(new OnDamageDealt(
              onlyByTriggerSource: false,
              creatureFilter: (c, s, _) => c == s.Ability.SourceCard));

          p.Effect = () => new CreateTokens(
            count: P(e => e.TriggerMessage<DamageDealtEvent>().Damage.Amount),
            token: Card
              .Named("Insect")
              .Power(1)
              .Toughness(1)
              .Type("Token Creature - Insect")
              .Text("{Flying}, {deathtouch}")
              .Colors(CardColor.Green)
              .SimpleAbilities(Static.Flying, Static.Deathtouch));
        });
    }
  }
}
