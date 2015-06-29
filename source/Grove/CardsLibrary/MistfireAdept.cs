namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class MistfireAdept : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mistfire Adept")
        .ManaCost("{3}{U}")
        .Type("Creature — Human Monk")
        .Text("{Prowess} {I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}{EOL}Whenever you cast a noncreature spell, target creature gains flying until end of turn.")
        .FlavorText("\"I don't know where my destiny lies, but I know it isn't here.\"")
        .Power(2)
        .Toughness(2)
        .Prowess()
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever you cast a noncreature spell, target creature gains flying until end of turn.";
          p.Trigger(new OnCastedSpell((c, ctx) =>
            c.Controller == ctx.You && !c.Is().Creature));

          p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Flying) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectBigWithoutEvasions(x => !x.Has().Flying));

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
