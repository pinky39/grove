namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class InspiredCharge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Inspired Charge")
        .ManaCost("{2}{W}{W}")
        .Type("Instant")
        .Text("Creatures you control get +2/+1 until end of turn.")
        .FlavorText("\"Impossible! How could they overwhelm us? We had barricades, war elephants, . . . and they were barely a tenth of our number!\"{EOL}—General Avitora")
        .Cast(p =>
        {
          p.Effect = () => new ApplyModifiersToPermanents((e, c) => c.Is().Creature, ControlledBy.SpellOwner,
                () => new AddPowerAndToughness(2, 1) { UntilEot = true }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new BeforeYouDeclareAttackers()));
        });
    }
  }
}
