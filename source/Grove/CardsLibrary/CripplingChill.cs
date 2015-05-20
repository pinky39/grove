namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class CripplingChill : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crippling Chill")
        .ManaCost("{2}{U}")
        .Type("Instant")
        .Text("Tap target creature. It doesn't untap during its controller's next untap step.{EOL}Draw a card.")
        .FlavorText("In the silence of the ice, even dreams become still.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new TapTargets(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddStaticAbility(Static.DoesNotUntap);

                  modifier.AddLifetime(new EndOfStep(
                    Step.Untap,
                    l => l.Modifier.SourceCard.Controller.IsActive));

                  return modifier;
                }),
              new DrawCards(1));

            p.TargetSelector.AddEffect(
              trg => trg.Is.Creature().On.Battlefield(),
              trg => trg.Message = "Select a creature to tap.");

            p.TargetingRule(new EffectGiveDoesNotUntap());
            p.TimingRule(new OnStep(Step.BeginningOfCombat));
          });
    }
  }
}