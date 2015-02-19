namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class FirehoofCavalry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Firehoof Cavalry")
        .ManaCost("{W}")
        .Type("Creature — Human Berserker")
        .Text("{3}{R}: Firehoof Cavalry gets +2/+0 and gains trample until end of turn.")
        .FlavorText("\"What warrior worth the name fears to leave a trail? If my enemies seek me, let them follow the ashes in my wake.\"")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{R}: Firehoof Cavalry gets +2/+0 and gains trample until end of turn.";
          p.Cost = new PayMana("{3}{R}".Parse(), supportsRepetitions: true);
          p.Effect = () => new ApplyModifiersToSelf(
            () => new AddPowerAndToughness(2, 0){UntilEot = true},
            () => new AddStaticAbility(Static.Trample) { UntilEot = true }).SetTags(EffectTag.IncreasePower);
          p.TimingRule(new PumpOwningCardTimingRule(2, 0));
          p.RepetitionRule(new RepeatMaxTimes());
        });
    }
  }
}
