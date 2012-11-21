namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class GraspOfDarkness : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Grasp of Darkness")
        .ManaCost("{B}{B}")
        .Type("Instant")
        .Text("Target creature gets -4/-4 until end of turn.")
        .FlavorText("On a world with five suns, night is compelled to become an aggressive force.")
        .Timing(Timings.InstantRemovalTarget())        
        .Effect<ApplyModifiersToTargets>(e =>
          {
            e.ToughnessReduction = 4;
            e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = -4;
                  m.Toughness = -4;
                }, untilEndOfTurn: true));
          })
        .Targets(
          selectorAi: TargetSelectorAi.ReduceToughness(4),
          effectValidator: TargetValidator(TargetIs.Creature()));
    }
  }
}