namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using Grove.UserInterface;

  public class OrderCards : Decision
  {
    private readonly Params _p = new Params();

    private OrderCards() {}

    public OrderCards(Player controller, Action<Params> setParameters)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<OrderCards, Ordering>
    {
      protected override bool ShouldExecuteQuery { get { return D._p.Cards.Count > 1; } }

      public override void ProcessResults()
      {
        D._p.ProcessDecisionResults.ProcessResults(Result);
      }

      protected override void SetResultNoQuery()
      {
        Result = D._p.Cards.Count == 0
          ? new Ordering()
          : new Ordering(0);
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new Ordering();
      }

      protected override void ExecuteQuery()
      {
        Result = D._p.ChooseDecisionResults.ChooseResult(D._p.Cards);
      }
    }

    public class Params
    {
      public List<Card> Cards;
      public IChooseDecisionResults<List<Card>, Ordering> ChooseDecisionResults;
      public IProcessDecisionResults<Ordering> ProcessDecisionResults;
      public string Title;
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (Ordering) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var dialog = Ui.Dialogs.CardOrder.Create(D._p.Cards, D._p.Title);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);
        Result = new Ordering(dialog.Ordering);
      }
    }
  }
}