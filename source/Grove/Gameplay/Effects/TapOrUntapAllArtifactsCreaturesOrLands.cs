namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class TapOrUntapAllArtifactsCreaturesOrLands : CustomizableEffect
  {
    private static readonly Dictionary<EffectOption, Func<Card, bool>> Selectors
      = new Dictionary<EffectOption, Func<Card, bool>>
        {
          {EffectOption.Creatures, card => card.Is().Creature},
          {EffectOption.Lands, card => card.Is().Land},
          {EffectOption.Artifacts, card => card.Is().Artifact}
        };

    private static readonly Dictionary<EffectOption, Action<Card>> Actions
      = new Dictionary<EffectOption, Action<Card>>
        {
          {EffectOption.Tap, card => card.Tap()},
          {EffectOption.Untap, card => card.Untap()}
        };

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      if (Target == Controller)
      {
        return new ChosenOptions(
          EffectOption.Untap,
          EffectOption.Creatures);
      }

      return Turn.Step == Step.Upkeep
        ? new ChosenOptions(
          EffectOption.Tap,
          EffectOption.Lands)
        : new ChosenOptions(
          EffectOption.Tap,
          EffectOption.Creatures);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var permanents = Target.Player().Battlefield.Where(Selectors[(EffectOption) results.Options[1]]);

      foreach (var permanent in permanents)
      {
        Actions[(EffectOption) results.Options[0]](permanent);
      }
    }

    public override string GetText()
    {
      return "#0 all #1 target player controls.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      yield return new DiscreteEffectChoice(
        EffectOption.Tap,
        EffectOption.Untap);

      yield return new DiscreteEffectChoice(
        EffectOption.Artifacts,
        EffectOption.Creatures,
        EffectOption.Lands);
    }
  }
}