namespace Grove.Core.Controllers
{
  using System;
  using System.Linq;
  using Results;

  public abstract class SacrificePermanents : Decision<ChosenCards>
  {
    public Func<Card, bool> Filter = delegate { return true; };
    public int Count { get; set; }
    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.Where(Filter).Count() > Count; } }

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
      {
        SacrificeAll();
        return;
      }

      foreach (var permanent in Result)
      {
        permanent.Sacrifice();
      }
    }

    private void SacrificeAll()
    {
      foreach (var creature in Controller.Battlefield.Where(Filter).ToList())
      {
        creature.Sacrifice();
      }
    }
  }
}