namespace Grove.Core.Controllers
{
  using System;
  using System.Linq;
  using Results;

  public abstract class ReturnPermanentsToHand : Decision<ChosenCards>
  {
    public Func<Card, bool> Filter = delegate { return true; };
    public int Count { get; set; }
    public string Text { get; set; }

    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.Where(Filter).Count() > Count; } }

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
      {
        ReturnAll();
        return;
      }

      foreach (var permanent in Result)
      {
        permanent.ReturnToHand();
      }
    }

    private void ReturnAll()
    {
      foreach (var permanent in Controller.Battlefield.Where(Filter).ToList())
      {
        permanent.ReturnToHand();
      }
    }
  }
}