namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions.Results;
  using Targeting;

  public class TapOrUntapAllArtifactsCreaturesOrLands : CustomizableEffect
  {
    private static readonly Dictionary<EffectChoiceOption, Func<Card, bool>> Selectors
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

    public override ChosenOptions ChooseOptions()
    {
      if (Target == Controller)
      {
        return new ChosenOptions(
          EffectChoiceOption.Untap,
          EffectChoiceOption.Creatures);
      }

      return Turn.Step == Step.Upkeep
        ? new ChosenOptions(
          EffectChoiceOption.Tap,
          EffectChoiceOption.Lands)
        : new ChosenOptions(
          EffectChoiceOption.Tap,
          EffectChoiceOption.Creatures);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var permanents = Target.Player().Battlefield.Where(Selectors[results.Options[1]]);

      foreach (var permanent in permanents)
      {
        Actions[results.Options[0]](permanent);
      }
    }

    public override string GetText()
    {
      return "#0 all #1 target player controls.";
    }

    public override IEnumerable<EffectChoice> GetChoices()
    {
      yield return new EffectChoice(EffectChoiceOption.Tap, EffectChoiceOption.Untap);
      yield return
        new EffectChoice(EffectChoiceOption.Artifacts, EffectChoiceOption.Creatures, EffectChoiceOption.Lands);
    }
  }
}