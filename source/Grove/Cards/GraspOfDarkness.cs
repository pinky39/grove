namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;

  public class GraspOfDarkness : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Grasp of Darkness")
        .ManaCost("{B}{B}")
        .Type("Instant")
        .Text("Target creature gets -4/-4 until end of turn.")
        .FlavorText("On a world with five suns, night is compelled to become an aggressive force.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(-4, -4) {UntilEot = true})
              {ToughnessReduction = 4};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new ReduceToughness(4));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}