namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ChasmSkulker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chasm Skulker")
        .ManaCost("{2}{U}")
        .Type("Creature - Squid Horror")
        .Text("Whenever you draw a card, put a +1/+1 counter on Chasm Skulker.{EOL}When Chasm Skulker dies, put X 1/1 blue Squid creature tokens with islandwalk onto the battlefield, where X is the number of +1/+1 counters on Chasm Skulker. {I}(They can't be blocked as long as defending player controls an Island.){/I}")
        .Power(1)
        .Toughness(1)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever you draw a card, put a +1/+1 counter on Chasm Skulker.";

          p.Trigger(new WhenPlayerDrawsCard((ability, player) => ability.OwningCard.Controller == player));

          p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1))
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Chasm Skulker dies, put X 1/1 blue Squid creature tokens with islandwalk onto the battlefield, where X is the number of +1/+1 counters on Chasm Skulker.";

          p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));

          p.Effect = () => new CreateTokens(
              count: P(e => e.Source.OwningCard.CountersCount(CounterType.PowerToughness)),
              token: Card
                .Named("Squid")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Squid")
                .Text("{Islandwalk}")
                .Colors(CardColor.Blue)
                .SimpleAbilities(Static.Islandwalk));
        });
    }
  }
}
