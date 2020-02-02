namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.CostRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class IcyBlast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Icy Blast")
        .ManaCost("{U}").HasXInCost()
        .Type("Instant")
        .Text("Tap X target creatures.{EOL}{I}Ferocious{/I} — If you control a creature with power 4 or greater, those creatures don't untap during their controllers' next untap steps.")
        .FlavorText("\"Do not think the sand or the sun will hold back the breath of winter.\"")
        .Cast(p =>
        {
          p.Effect = () => new FerociousEffect(
            normal: new Effect[]
            {
              new TapTargets(), 
            },
            ferocious: new Effect[]
            {
              new TapTargets(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddStaticAbility(Static.DoesNotUntap);

                  modifier.AddLifetime(new EndOfStep(
                    Step.Untap,
                    l => l.Modifier.SourceCard.Controller.IsActive));

                  return modifier;
                }),
            });

          p.TargetSelector.AddEffect(
            trg => trg.Is.Creature().On.Battlefield(),
            trg => {
              trg.MinCount = Value.PlusX;
              trg.MaxCount = Value.PlusX;            
          });

          p.TimingRule(new OnStep(Step.BeginningOfCombat));
          p.CostRule(new XIsOpponentsCreatureCount());
          p.TargetingRule(new EffectTapCreature());
        });
    }
  }
}
