namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class MarshCasualties : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Marsh Casualties")
        .ManaCost("{B}{B}")
        .KickerCost("{3}")
        .Type("Sorcery")
        .Text(
          "{Kicker} {3}{EOL}Creatures target player controls get -1/-1 until end of turn. If Marsh Casualties was kicked, those creatures get -2/-2 until end of turn instead.")        
        .Timing(Timings.MainPhases())
        .Effect<ApplyModifiersToCreatures>(p =>
          {
            p.Effect.ToughnessReduction = 1;            
            p.Effect.Modifiers(p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
              {
                m.Power = -1;
                m.Toughness = -1;
              }, untilEndOfTurn: true));
          })
        .Targets(
          selectorAi: TargetSelectorAi.Opponent(),
          effectValidator: C.Validator(Validators.Player()))
        .KickerEffect<ApplyModifiersToCreatures>(p =>
          {
            p.Effect.ToughnessReduction = 2;     
            p.Effect.Modifiers(p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
              {
                m.Power = -2;
                m.Toughness = -2;
              }, untilEndOfTurn: true));
          })
        .KickerTargets(
          aiTargetSelector: TargetSelectorAi.Opponent(),
          effectValidators: C.Validator(Validators.Player()));
    }
  }
}