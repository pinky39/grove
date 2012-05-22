namespace Grove.Core.Controllers
{
  using System.Linq;
  using Results;

  public abstract class SacrificeCreatures : Decision<ChosenCards>
  {
    public int Count { get; set; }    

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
      {
        SacrificeAll();
        return;
      }

      foreach (var creature in Result)
      {
        Player.SacrificeCard(creature);
      }
    }

    private void SacrificeAll()
    {
      foreach (var creature in Player.Battlefield.Creatures.ToList())
      {
        Player.SacrificeCard(creature);
      }
    }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        return Player.Battlefield.Creatures.Count() > Count;
      }
    }    
  }
}