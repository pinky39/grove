namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;

  public class DestroyAllLandsOrCreatures : CustomizableEffect
  {
    public DestroyAllLandsOrCreatures()
    {
      SetTags(EffectTag.Destroy);
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      var opponentCreatureCount = Controller.Opponent.Battlefield.Creatures.Count();
      var yourCreatureCount = Controller.Battlefield.Creatures.Count();

      return opponentCreatureCount - yourCreatureCount > 0
        ? new ChosenOptions(EffectOption.Creatures)
        : new ChosenOptions(EffectOption.Lands);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      Func<Card, bool> filter;

      if (results.Options[0].Equals(EffectOption.Lands))
      {
        filter = c => c.Is().Land;
      }
      else
      {
        filter = c => c.Is().Creature;
      }

      var permanents = Players.Permanents().Where(filter).ToList();

      foreach (var permanent in permanents)
      {
        permanent.Destroy(allowToRegenerate: false);
      }
    }

    public override string GetText()
    {
      return "Destroy all #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      yield return new DiscreteEffectChoice(
        EffectOption.Lands,
        EffectOption.Creatures);
    }
  }
}