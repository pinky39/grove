namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.CostRules;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class Outmaneuver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Outmaneuver")
        .ManaCost("{R}").HasXInCost()
        .Type("Instant")
        .Text("X target blocked creatures assign their combat damage this turn as though they weren't blocked.")
        .FlavorText("Push one goblin into sight, an' run a lot. That's tactics.")
        .Cast(p =>
          {
            p.Text = "X target blocked creatures assign their combat damage this turn as though they weren't blocked.";
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.AssignsDamageAsThoughItWasntBlocked) {UntilEot = true});

            p.TargetSelector.AddEffect(trg =>
              {
                trg.MinCount = Value.PlusX;
                trg.MaxCount = Value.PlusX;
                trg.Is.Card(c => c.HasBlockers).On.Battlefield();
              });


            p.TimingRule(new OnYourTurn(Step.DeclareBlockers));
            p.TimingRule(new WhenYouHavePermanents(selector: c => c.HasBlockers));

            p.CostRule(new XIsNumOfBlockedAttackers());
            p.TargetingRule(new EffectBigWithoutEvasions());
          });
    }
  }
}