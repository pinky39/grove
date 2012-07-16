namespace Grove.Core.Controllers
{
  using System.Linq;
  using Results;

  public abstract class SacrificeCreatures : Decision<ChosenCards>
  {
    public int Count { get; set; }
    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.Creatures.Count() > Count; } }

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
      {
        SacrificeAll();
        return;
      }

      foreach (var creature in Result)
      {
        Controller.SacrificeCard(creature);
      }
    }

    private void SacrificeAll()
    {
      foreach (var creature in Controller.Battlefield.Creatures.ToList())
      {
        Controller.SacrificeCard(creature);
      }
    }
  }
}