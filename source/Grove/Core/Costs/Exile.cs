namespace Grove.Costs
{
  using System.Linq;

  public class Exile : Cost
  {
    private readonly bool _fromGraveyard;

    private Exile() {}

    public Exile(bool fromGraveyard = false)
    {
      _fromGraveyard = fromGraveyard;
    }

    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      if (Validator != null)
      {
        return _fromGraveyard
          ? Card.Controller.Graveyard.Any(
            card => Validator.IsTargetValid(card, Card))
          : Card.Controller.Battlefield.Any(
            permanent => Validator.IsTargetValid(permanent, Card));
      }

      return _fromGraveyard
        ? Card.Zone == Zone.Graveyard
        : Card.IsPermanent;
    }

    public override void PayPartial(PayCostParameters p)
    {
      var target = p.Targets.Cost.FirstOrDefault();

      if (target != null)
      {
        target.Card().Exile();
        return;
      }

      Card.Exile();
    }
  }
}