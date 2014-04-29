namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Events;
  using Triggers;

  public class PhyrexianNegator : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Negator")
        .ManaCost("{2}{B}")
        .Type("Creature Horror")
        .Text("{Trample}{EOL}Whenever Phyrexian Negator is dealt damage, sacrifice that many permanents.")
        .FlavorText("They exist to cease.")
        .Power(5)
        .Toughness(5)
        .SimpleAbilities(Static.Trample)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever Phyrexian Negator is dealt damage, sacrifice that many permanents.";

            p.Trigger(new OnDamageDealt(
              onlyByTriggerSource: false,
              creatureFilter: (c, s, _) => c == s.Ability.SourceCard));

            p.Effect = () => new PlayerSacrificePermanents(
              count: P(e => e.TriggerMessage<DamageDealtEvent>().Damage.Amount),
              player: P(e => e.Controller),
              filter: c => true,
              text: "Select permanents to sacrifice.");

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}