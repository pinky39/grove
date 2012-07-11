namespace Grove.Core.Controllers.Scenario
{
  using Details.Cards;
  using Details.Mana;

  public class ScenarioAbility : Results.Ability
  {
    public ScenarioAbility(Card card, ActivationParameters activationParameters, int index)
      : base(card, activationParameters, index) {}

    public override bool CanPlay()
    {
      var manaCost = Card.GetActivatedAbilityManaCost(Index);

      if (ActivationParameters.X.HasValue)
        manaCost = manaCost.Add(ActivationParameters.X.Value.AsColorlessMana());

      Card.Controller.AddManaToManaPool(manaCost);

      var prerequisites = Card.CanActivateAbility(Index);
      return prerequisites.CanBeSatisfied;
    }
  }
}