namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Decisions.Results;

  public class DestroyAllLandsOrCreatures : CustomizableEffect
  {
    public DestroyAllLandsOrCreatures()
    {
      Category = EffectCategories.Destruction;
    }
    
    public override ChosenOptions ChooseOptions()
    {
      var opponentCreatureCount = Controller.Opponent.Battlefield.Creatures.Count();
      var yourCreatureCount = Controller.Battlefield.Creatures.Count();

      return opponentCreatureCount - yourCreatureCount > 0
        ? new ChosenOptions(EffectChoiceOption.Creatures)
        : new ChosenOptions(EffectChoiceOption.Lands);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      if (results.Options[0] == EffectChoiceOption.Lands)
      {
        Players.DestroyPermanents(card => card.Is().Land);
        return;
      }

      Players.DestroyPermanents(
        card => card.Is().Creature, allowToRegenerate: false);
    }

    public override string GetText()
    {
      return "Destroy all #0.";
    }

    public override IEnumerable<EffectChoice> GetChoices()
    {
      yield return new EffectChoice(EffectChoiceOption.Lands, EffectChoiceOption.Creatures);
    }
  }
}