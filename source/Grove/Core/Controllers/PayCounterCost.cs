namespace Grove.Core.Controllers
{
  using Details.Cards.Effects;
  using Details.Mana;
  using Results;

  public abstract class PayCounterCost : Decision<BooleanResult>
  {
    public IManaAmount DoNotCounterCost { get; set; }
    public bool TapLandsEmptyManaPool { get; set; }
    public int? Lifeloss { get; set; }
    public Effect Spell { get; set; }

    protected override bool ShouldExecuteQuery { get { return DoNotCounterCost != null && Controller.HasMana(DoNotCounterCost); } }

    public override void ProcessResults()
    {
      if (Result.IsTrue)
      {
        Controller.Consume(DoNotCounterCost, ManaUsage.Any);
        return;
      }

       if (Lifeloss.HasValue)
      {
        Controller.Life -= Lifeloss.Value;
      }

      if (TapLandsEmptyManaPool)
      {
        foreach (var land in Controller.Battlefield.Lands)
        {
          land.Tap();
        }

        Controller.EmptyManaPool();
      }

      Game.Stack.Counter(Spell);
    }
  }
}