namespace Grove.Gameplay.Effects
{
  using System;
  using Card.Counters;
  using Decisions;
  using Decisions.Results;
  using Modifiers;

  public class ChooseToAddCounter : Effect, IChooseDecisionResults<BooleanResult>,
    IProcessDecisionResults<BooleanResult>
  {
    private readonly Func<Effect, bool> _chooseAi;

    private ChooseToAddCounter() {}

    public ChooseToAddCounter(Func<Effect, bool> chooseAi)
    {
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
            Target = Source.OwningCard,
            SourceCard = Source.OwningCard,
            SourceEffect = this,
          };

        var addCounter = new AddCounters(() => new ChargeCounter(), 1);
        addCounter.Initialize(p, Game);

        Source.OwningCard.AddModifier(addCounter);
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue<ChooseTo>(Controller, p =>
        {
          p.Text = string.Format("{0}: Add a counter?", Source.OwningCard.Name);
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        });
    }
  }
}