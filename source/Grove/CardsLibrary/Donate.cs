namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;

  public class Donate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Donate")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text("Opponent gains control of target permanent you control.")
        .FlavorText("Campus pranksters initiate new students with the old 'beeble bomb' routine.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new ChangeController(m => m.SourceCard.Controller.Opponent));

            p.TargetSelector.AddEffect(trg => trg.Is
              .Card(controlledBy: ControlledBy.SpellOwner).On.Battlefield());

            p.TargetingRule(new EffectRankBy(x => x.Is().Land ? 100 : -x.Score));
          });
    }
  }
}