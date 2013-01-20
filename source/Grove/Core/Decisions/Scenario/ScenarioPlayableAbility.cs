namespace Grove.Core.Decisions.Scenario
{
  using System.Linq;
  using Mana;

  public class ScenarioPlayableAbility : Results.PlayableAbility
  {
    public ScenarioPlayableAbility(Card card, ActivationParameters activationParameters, int index)
      : base(card, activationParameters, index) {}

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