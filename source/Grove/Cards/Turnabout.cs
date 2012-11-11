namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Controllers.Results;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Turnabout : CardsSource
  {
    private static readonly Dictionary<EffectChoiceOption, Func<Card, bool>> Filters
      = new Dictionary<EffectChoiceOption, Func<Card, bool>>
        {
          {EffectChoiceOption.Creatures, card => card.Is().Creature},
          {EffectChoiceOption.Lands, card => card.Is().Land},
          {EffectChoiceOption.Artifacts, card => card.Is().Artifact}
        };

    private static readonly Dictionary<EffectChoiceOption, Action<Card>> Actions
      = new Dictionary<EffectChoiceOption, Action<Card>>
        {
          {EffectChoiceOption.Tap, card => card.Tap()},
          {EffectChoiceOption.Untap, card => card.Untap()}
        };

    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Turnabout")
        .ManaCost("{2}{U}{U}")
        .Type("Instant")
        .Text(
          "Choose artifact, creature, or land. Tap all untapped permanents of the chosen type target player controls, or untap all tapped permanents of that type that player controls.")
        .FlavorText("The best cure for a big ego is a little failure.")
        .Timing(Timings.NoRestrictions())
        .Targets(
          selectorAi:
            Any(TargetSelectorAi.TapOpponentsCreatures(), TargetSelectorAi.TapOpponentsLands(),
              TargetSelectorAi.UntapYourCreatures()),
          effectValidator: Validator(Validators.Player(), text: "Select a player.")
        )
        .Effect<CustomizableEffect>(e =>
          {
            e.Choices(
              Choice(EffectChoiceOption.Tap, EffectChoiceOption.Untap),
              Choice(EffectChoiceOption.Artifacts, EffectChoiceOption.Creatures, EffectChoiceOption.Lands)
              );

            e.Ai = p =>
              {
                if (p.Effect.Target() == p.Controller)
                {
                  return new ChosenOptions(
                    EffectChoiceOption.Untap,
                    EffectChoiceOption.Creatures);
                }

                return p.Game.Turn.Step == Step.Upkeep
                  ? new ChosenOptions(
                    EffectChoiceOption.Tap,
                    EffectChoiceOption.Lands)
                  : new ChosenOptions(
                    EffectChoiceOption.Tap,
                    EffectChoiceOption.Creatures);
              };

            e.Text = "#0 all #1 target player controls.";

            e.ProcessResults =
              p =>
                {
                  var permanents = p.Effect.Target().Player().Battlefield
                    .Where(Filters[p.Result.Options[1]]);

                  foreach (var permanent in permanents)
                  {
                    Actions[p.Result.Options[0]](permanent);
                  }
                };
          });
    }
  }
}