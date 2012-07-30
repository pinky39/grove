namespace Grove.Core.Controllers
{
  using System.Linq;
  using Results;

  public abstract class SacrificePermanents : Decision<ChosenCards>
  {
    public string PermanentType;
    public int Count { get; set; }
    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.Where(x => x.Is(PermanentType)).Count() > Count; } }

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
      foreach (var creature in Controller.Battlefield.Where(x => x.Is(PermanentType)).ToList())
      {
        creature.Sacrifice();
      }
    }
  }
}