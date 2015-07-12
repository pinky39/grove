namespace Grove.Effects
{
  using Decisions;

  public class CounterThatSpell : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly DynParam<Effect> _spell;
    private readonly DynParam<int> _doNotCounterCost;

    private CounterThatSpell() {}

    public CounterThatSpell(DynParam<Effect> spell, DynParam<int> doNotCounterCost = null)
    {
      _spell = spell;
      _doNotCounterCost = doNotCounterCost;

      RegisterDynamicParameters(_doNotCounterCost, _spell);
    }

    protected override void ResolveEffect()
    {
      if (_doNotCounterCost == null)
      {
        CounterSpell(_spell.Value);
        return;
      }

      Enqueue(new PayOr(_spell.Value.Controller, p =>
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

      CounterSpell(_spell.Value);
    }

    private void CounterSpell(Effect spell)
    {
      if (spell == null || !Stack.HasSpellWithSource(spell.Source.SourceCard))
        return;

      Stack.Counter(spell);
    }
  }
}