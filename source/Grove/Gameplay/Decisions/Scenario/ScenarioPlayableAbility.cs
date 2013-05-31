namespace Grove.Gameplay.Decisions.Scenario
{
  using System;
  using System.Linq;
  using Results;

  [Serializable]
  public class ScenarioPlayableAbility : PlayableAbility
  {
    public override bool CanPlay()
    {
      var manaCost = Card.GetActivatedAbilityManaCost(Index);

      if (ActivationParameters.X.HasValue)
        manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());

      Card.Controller.AddManaToManaPool(manaCost);

      var prerequisites = Card.CanActivateAbilities();

      return prerequisites.Any(x => x.Index == Index);
    }
  }
}