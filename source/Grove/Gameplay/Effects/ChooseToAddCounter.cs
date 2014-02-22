namespace Grove.Gameplay.Effects
{
  using System;
  using Decisions;
  using Modifiers;

  public class ChooseToAddCounter : Effect, IChooseDecisionResults<BooleanResult>,
    IProcessDecisionResults<BooleanResult>
  {
    private readonly Func<Effect, bool> _chooseAi;
    private readonly CounterType _counterType;

    private ChooseToAddCounter() {}

    public ChooseToAddCounter(CounterType counterType, Func<Effect, bool> chooseAi)
    {
      _counterType = counterType;
      _chooseAi = chooseAi;
    }

    public BooleanResult ChooseResult()
    {
      return _chooseAi(this);
    }

    public void ProcessResults(BooleanResult results)
    {
      if (results.IsTrue)
      {
        var p = new ModifierParameters
          {
            SourceCard = Source.OwningCard,
            SourceEffect = this,
          };

        var addCounter = new AddCounters(() => new SimpleCounter(_counterType), 1);
        Source.OwningCard.AddModifier(addCounter, p);
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue(new ChooseTo(Controller, p =>
        {
          p.Text = string.Format("{0}: Add a counter?", Source.OwningCard.Name);
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }));
    }
  }
}