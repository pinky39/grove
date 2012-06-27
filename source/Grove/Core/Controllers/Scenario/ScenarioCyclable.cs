namespace Grove.Core.Controllers.Scenario
{
  using System;
  using Results;

  public class ScenarioCyclable : Cyclable
  {
    public ScenarioCyclable(Card card) : base(card) {}

    private Player Controller { get { return Card.Controller; } }

    public override bool CanPlay()
    {
      if (Card == null)
        throw new InvalidOperationException("Did you forget to add card to players hand?");

      Controller.AddManaToManaPool(Card.CyclingCost);
      var prerequisites = Card.CanCycle();
      return prerequisites.CanBeSatisfied;
    }
  }
}