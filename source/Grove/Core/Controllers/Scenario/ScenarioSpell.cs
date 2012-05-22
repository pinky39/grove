namespace Grove.Core.Controllers.Scenario
{
  using Results;

  public class ScenarioSpell : Spell
  {
    public ScenarioSpell(Card card, ActivationParameters activationParameters) : base(card, activationParameters) {}

    private Player Controller { get { return Card.Controller; } }

    public override bool CanPlay()
    {
      var manaCost = ManaAmount.Zero;
      
      if (Card.ManaCost != null)
      {
        manaCost = manaCost + Card.ManaCost;
      }
                
      if (Card.KickerCost != null)
      {
        manaCost = manaCost + Card.KickerCost;
      }


      if (ActivationParameters.X.HasValue)
        manaCost = manaCost + ActivationParameters.X.Value;

      Controller.AddManaToManaPool(manaCost);
      var prerequisites = Card.CanCast();
      return prerequisites.CanBeSatisfied;
    }
  }
}