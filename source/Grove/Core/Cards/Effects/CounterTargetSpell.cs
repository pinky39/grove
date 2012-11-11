namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;
  using Controllers.Results;
  using Mana;
  using Targeting;

  public class CounterTargetSpell : Effect, IProcessDecisionResults<BooleanResult>
  {
    public int? ControllersLifeloss;
    public IManaAmount DoNotCounterCost;
    public bool TapLandsEmptyPool;

    public void ResultProcessed(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      CounterSpell();
    }

    protected override void ResolveEffect()
    {
      var targetSpellController = Target().Effect().Controller;

      if (DoNotCounterCost == null)
      {
        CounterSpell();
        return;
      }

      Game.Enqueue<PayOr>(targetSpellController, p =>
        {
          p.ManaAmount = DoNotCounterCost;
          p.Text = FormatText(string.Format("Pay {0}?", DoNotCounterCost));
          p.ProcessDecisionResults = this;
        });
      return;
    }

    private void CounterSpell()
    {
      if (ControllersLifeloss.HasValue)
      {
        Controller.Life -= ControllersLifeloss.Value;
      }

      if (TapLandsEmptyPool)
      {
        foreach (var land in Controller.Battlefield.Lands)
        {
          land.Tap();
        }

        Controller.EmptyManaPool();
      }

      Game.Stack.Counter(Target().Effect());
    }
  }
}