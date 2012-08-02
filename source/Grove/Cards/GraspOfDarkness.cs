namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class GraspOfDarkness : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Grasp of Darkness")
        .ManaCost("{B}{B}")
        .Type("Instant")
        .Text("Target creature gets -4/-4 until end of turn.")
        .FlavorText("On a world with five suns, night is compelled to become an aggressive force.")
        .Timing(Timings.TargetRemovalInstant())        
        .Effect<ApplyModifiersToTargets>(p =>
          {
            p.Effect.ToughnessReduction = 4;
            p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = -4;
                  m.Toughness = -4;
                }, untilEndOfTurn: true));
          })
        .Targets(
          selectorAi: TargetSelectorAi.ReduceToughness(4),
          effectValidator: C.Validator(Validators.Creature()));
    }
  }
}