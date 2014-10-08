namespace Grove.Effects
{
  using Decisions;

  public class CounterTopSpell : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly DynParam<int> _doNotCounterCost;

    private CounterTopSpell() {}

    public CounterTopSpell(DynParam<int> doNotCounterCost = null)
    {
      _doNotCounterCost = doNotCounterCost;

      RegisterDynamicParameters(doNotCounterCost);
    }

    protected override void ResolveEffect()
    {
      if (Stack.TopSpell == null)
        return;

      if (_doNotCounterCost == null)
      {
        CounterSpell();
        return;
      }

      Enqueue(new PayOr(Stack.TopSpell.Controller, p =>
      {
        p.ManaAmount = _doNotCounterCost.Value.Colorless();
        p.Text = string.Format("Pay {0}?", _doNotCounterCost);
        p.ProcessDecisionResults = this;
      }));

    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      CounterSpell();
    }

    private void CounterSpell()
    {
      if (Stack.TopSpell != null)
        Stack.Counter(Stack.TopSpell);
    }
  }
}