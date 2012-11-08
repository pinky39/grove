namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Humble : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Humble")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Target creature loses all abilities and becomes 0/1 until end of turn.")
        .FlavorText("'It is not your place to rule, Radiant. It may not even be mine.'{EOL}—Serra")
        .Timing(Timings.InstantRemovalTarget(combatOnly: true))        
        .Effect<ApplyModifiersToTargets>(e =>
          {
            e.ToughnessReduction = e.Target().Card().Toughness.Value - 1;
            e.Modifiers(
              Modifier<DisableAbilities>(untilEndOfTurn: true),
              Modifier<SetPowerAndToughness>(m =>
                {
                  m.Power = 0;
                  m.Tougness = 1;
                }, untilEndOfTurn: true)
              );
          })
        .Targets(
          selectorAi: TargetSelectorAi.Destroy(),
          effectValidator: Validator(Validators.Creature()));
    }
  }
}