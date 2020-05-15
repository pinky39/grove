namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Modifiers;
  using Triggers;

  public class AltacBloodseeker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Altac Bloodseeker")
        .ManaCost("{1}{R}")
        .Type("Creature — Human Berserker")
        .Text(
          "Whenever a creature an opponent controls dies, Altac Bloodseeker gets +2/+0 and gains first strike and haste until end of turn.{I}(It deals combat damage before creatures without first strike, and it can attack and {T} as soon as it comes under your control.){/I}")
        .Power(2)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a creature an opponent controls dies, Altac Bloodseeker gets +2/+0 and gains first strike and haste until end of turn.";

            p.Effect = () => new ApplyModifiersToSelf(
              () => new AddPowerAndToughness(2, 0) {UntilEot = true},
              () => new AddSimpleAbility(Static.FirstStrike) {UntilEot = true},
              () => new AddSimpleAbility(Static.Haste) {UntilEot = true}).SetTags(EffectTag.IncreasePower,
                EffectTag.IncreaseToughness);

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield, to: Zone.Graveyard,
              selector: (c, ctx) => c.Is().Creature && c.Controller == ctx.Opponent));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}