namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Abilities;
  using Decisions.Results;
  using Modifiers;
  using Targeting;

  public class TargetLoosesChosenAbility : CustomizableEffect
  {
    private readonly List<Static> _choices = new List<Static>();

    private TargetLoosesChosenAbility() {}

    public TargetLoosesChosenAbility(params Static[] choices)
    {
      _choices.AddRange(choices);
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      var target = Target.Card();

      foreach (var choice in _choices)
      {
        if (target.Has().Has(choice))
        {
          return new ChosenOptions(choice.ToString());
        }
      }

      return new ChosenOptions(_choices[0].ToString());
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var ability = (Static) Enum.Parse(
        typeof (Static),
        (string) results.Options[0]);

      var modifier = new DisableAbility(ability) {UntilEot = true};

      var mp = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard
        };

      Target.Card().AddModifier(modifier, mp);
    }

    public override string GetText()
    {
      return "Target creature looses #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      yield return new DiscreteEffectChoice(
        _choices.Select(x => x.ToString()).ToArray());
    }
  }
}