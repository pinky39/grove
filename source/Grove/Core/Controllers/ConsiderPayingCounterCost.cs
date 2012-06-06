namespace Grove.Core.Controllers
{
  using Effects;
  using Results;
  using Zones;

  public abstract class ConsiderPayingLifeOrMana: Decision<BooleanResult>
  {
    public Effect Effect { get; set; }
    public PayLifeOrManaHandler Handler { get; set; }
    public int? Life { get; set; }
    public IManaAmount Mana { get; set; }
    public Stack Stack { get; set; }

    public override void ProcessResults()
    {      
      Handler(new PayLifeOrManaArguments{
        Answer = Result.IsTrue,
        Effect = Effect,
        Stack = Stack,
        Player = Player
      });            
    }
  }
}