namespace Grove.Core.Effects
{
  using Ai;
  using Decisions;
  using Decisions.Results;
  using Mana;
  using Targeting;

  public class CounterTargetSpell : Effect, IProcessDecisionResults<BooleanResult>
  {
    private readonly int? _controllerLifeloss;
    private readonly IManaAmount _doNotCounterCost;
    private readonly bool _tapLandsAndEmptyManaPool;

    public CounterTargetSpell(int? controllerLifeloss = null, IManaAmount doNotCounterCost = null, bool tapLandsAndEmptyManaPool = false)
    {
      _controllerLifeloss = controllerLifeloss;
      _doNotCounterCost = doNotCounterCost;
      _tapLandsAndEmptyManaPool = tapLandsAndEmptyManaPool;
      Category = EffectCategories.Counterspell;
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
        return;

      CounterSpell();
    }

    protected override void ResolveEffect()
    {
      var targetSpellController = Target.Effect().Controller;

      if (_doNotCounterCost == null)
      {
        CounterSpell();
        return;
      }

      Game.Enqueue<PayOr>(targetSpellController, p =>
        {
          p.ManaAmount = _doNotCounterCost;
          p.Text = FormatText(string.Format("Pay {0}?", _doNotCounterCost));
          p.ProcessDecisionResults = this;
        });
      return;
    }

    private void CounterSpell()
    {
      if (_controllerLifeloss.HasValue)
      {
        Controller.Life -= _controllerLifeloss.Value;
      }

      if (_tapLandsAndEmptyManaPool)
      {
        foreach (var land in Controller.Battlefield.Lands)
        {
          land.Tap();
        }

        Controller.EmptyManaPool();
      }

      Game.Stack.Counter(Target.Effect());
    }
  }
}