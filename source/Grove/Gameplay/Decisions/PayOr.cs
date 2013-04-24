﻿namespace Grove.Gameplay.Decisions
{
  using System;
  using Mana;
  using Results;

  public abstract class PayOr : Decision<BooleanResult>
  {
    public Func<PayOr, bool> Ai = delegate { return true; };
    public IManaAmount ManaAmount { get; set; }
    public int? Life { get; set; }
    public string Text { get; set; }
    public ManaUsage ManaUsage = ManaUsage.Any;

    public IProcessDecisionResults<BooleanResult> ProcessDecisionResults { get; set; }

    protected override bool ShouldExecuteQuery { get { return CanPay(); } }

    private bool CanPay()
    {
      if (ManaAmount != null)
      {
        return Controller.HasMana(ManaAmount, ManaUsage);
      }

      if (Life.HasValue)
      {
        return true;
      }

      return false;
    }

    public override void ProcessResults()
    {
      Result = Result ?? false;
      
      if (Result.IsTrue)
      {
        Pay();        
      }

      if (ProcessDecisionResults != null)
        ProcessDecisionResults.ProcessResults(Result);
    }

    private void Pay()
    {
      if (ManaAmount != null)
      {
        Controller.Consume(ManaAmount, ManaUsage);
        return;
      }

      if (Life.HasValue)
      {
        Controller.Life -= Life.Value;
      }
    }
  }
}