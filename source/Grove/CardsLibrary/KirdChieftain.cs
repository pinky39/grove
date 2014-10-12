namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class KirdChieftain : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kird Chieftain")
        .ManaCost("{3}{R}")
        .Type("Creature — Ape")
        .Text("Kird Chieftain gets +1/+1 as long as you control a Forest.{EOL}{4}{G}: Target creature gets +2/+2 and gains trample until end of turn. {I}(If it would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(
            to: Zone.Battlefield,
            filter: (card, ability, _) =>
            {
              var count = ability.OwningCard.Controller.Battlefield.Count(c => c.Is("Forest"));

              // Kird Chieftain comes into battlefield
              if (ability.OwningCard == card && count > 0)
                return true;

              return ability.OwningCard.Zone == Zone.Battlefield &&
                ability.OwningCard.Controller == card.Controller && card.Is("Forest") && count == 1;
            }));

          p.UsesStack = false;

          p.Effect = () => new ApplyModifiersToSelf(
            () =>
            {
              var modifier = new AddPowerAndToughness(1, 1);
              modifier.AddLifetime(new OwnerControlsPermamentsLifetime(c => c.Is("Forest")));
              return modifier;
            });
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{4}{G}: Target creature gets +2/+2 and gains trample until end of turn.";

          p.Cost = new PayMana("{4}{G}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);

          p.Effect = () => new ApplyModifiersToTargets(
            () => new AddPowerAndToughness(2, 2){UntilEot = true},
            () => new AddStaticAbility(Static.Trample){UntilEot = true});

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TimingRule(new BeforeYouDeclareAttackers());
          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          p.RepetitionRule(new RepeatMaxTimes());
        });
    }
  }
}
