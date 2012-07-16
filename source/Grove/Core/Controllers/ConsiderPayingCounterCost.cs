namespace Grove.Core.Controllers
{
  using Details.Mana;
  using Results;

  public abstract class ConsiderPayingLifeOrMana : Decision<BooleanResult>
  {
    public object Context { get; set; }
    public PayLifeOrManaHandler Handler { get; set; }
    public int? Life { get; set; }
    public IManaAmount Mana { get; set; }
    public string Message { get; set; }    

    public override void ProcessResults()
    {
      Handler(new PayLifeOrManaArguments(Result.IsTrue, Controller, Game, Context));
    }
  }
}