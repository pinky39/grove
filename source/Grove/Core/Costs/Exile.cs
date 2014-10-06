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

    protected override void CanPay(CanPayResult result)
    {
      if (Validator != null)
      {
        if (_fromGraveyard)
        {
          result.CanPay(Card.Controller.Graveyard.Any(
            card => Validator.IsTargetValid(card, Card)));
        }
        else
        {
          result.CanPay(Card.Controller.Battlefield.Any(
            permanent => Validator.IsTargetValid(permanent, Card)));
        }        

        return;
      }

      if (_fromGraveyard)
      {
        result.CanPay(() => Card.Zone == Zone.Graveyard);
      }
      else
      {
        result.CanPay(() => Card.IsPermanent);
      }      
    }

    protected override void PayCost(Targets targets, int? x, int repeat)
    {
      var target = targets.Cost.FirstOrDefault();

      if (target != null)
      {
        target.Card().Exile();
        return;
      }

      Card.Exile();
    }
  }
}