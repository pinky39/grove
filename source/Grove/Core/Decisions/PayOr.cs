namespace Grove.Decisions
{
  using System;
  using System.Windows;
  using Grove.UserInterface;

  public class PayOr : Decision
  {
    private readonly Params _p = new Params();

    private PayOr() {}

    public PayOr(Player controller, Action<Params> setParameters)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<PayOr, BooleanResult>
    {
      protected override bool ShouldExecuteQuery { get { return CanPay(); } }

      public override void ProcessResults()
      {
        if (Result.IsTrue)
        {
          Pay();
        }

        if (D._p.ProcessDecisionResults != null)
          D._p.ProcessDecisionResults.ProcessResults(Result);
      }

      private bool CanPay()
      {
        if (D._p.ManaAmount != null)
        {
          return D.Controller.HasMana(D._p.ManaAmount, D._p.ManaUsage);
        }

        if (D._p.Life.HasValue)
        {
          return true;
        }

        return false;
      }

      protected override void SetResultNoQuery()
      {
        Result = false;
      }

      private void Pay()
      {
        if (D._p.ManaAmount != null)
        {
          D.Controller.Consume(D._p.ManaAmount, D._p.ManaUsage);
          return;
        }

        if (D._p.Life.HasValue)
        {
          D.Controller.Life -= D._p.Life.Value;
        }
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = false;
      }

      protected override void ExecuteQuery()
      {
        if (D._p.ChooseDecisionResults != null)
        {
          Result = D._p.ChooseDecisionResults.ChooseResult();
          return;
        }

        Result = true;
      }
    }

    public class Params
    {
      public IChooseDecisionResults<BooleanResult> ChooseDecisionResults;
      public int? Life;
      public IManaAmount ManaAmount;
      public ManaUsage ManaUsage = ManaUsage.Any;
      public IProcessDecisionResults<BooleanResult> ProcessDecisionResults;
      public string Text;
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (BooleanResult) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var result = Ui.Shell.ShowMessageBox(
          message: D._p.Text,
          buttons: MessageBoxButton.YesNo,
          type: DialogType.Small);

        Result = result == MessageBoxResult.Yes;
      }
    }
  }
}