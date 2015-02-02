namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using UserInterface;
  using UserInterface.SelectTarget;

  public class SplitCards : Decision
  {
    private readonly Params _p = new Params();

    private SplitCards() {}

    public SplitCards(Player controller, Action<Params> setParameters)
      : base(
        controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(),
        () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    public class Params
    {
      public List<Card> Cards;
      public IChooseDecisionResults<List<Card>, Split> ChooseDecisionResults;
      public IProcessDecisionResults<Split> ProcessDecisionResults;
      public string SplitText;
      public string PositiveText;
      public string NegativeText;
    }

    private abstract class Handler : DecisionHandler<SplitCards, Split>
    {
      protected override bool ShouldExecuteQuery
      {
        get { return D._p.Cards.Count > 0; }
      }

      public override void ProcessResults()
      {
        D._p.ProcessDecisionResults.ProcessResults(Result);
      }

      protected override void SetResultNoQuery()
      {
        Result = new Split();
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new Split();
      }

      protected override void ExecuteQuery()
      {
        Result = D._p.ChooseDecisionResults.ChooseResult(D._p.Cards);
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery
      {
        get { return true; }
      }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (Split) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var validatorParameters = new TargetValidatorParameters
          {
            IsValidZone = p => p.Zone == D._p.Cards[0].Zone && p.ZoneOwner == D.Controller,
            IsValidTarget = p => D._p.Cards.Contains((Card) p.Target),
            MinCount = 0,
            MaxCount = D._p.Cards.Count,
            Message = D._p.SplitText,
          };

        var validator = new TargetValidator(validatorParameters);
        validator.Initialize(Game, D.Controller);

        var selectTargetParameters = new SelectTargetParameters
          {
            Validator = validator,
            Instructions = "(Press enter when done.)"
          };

        var selectDialog = Ui.Dialogs.SelectTarget.Create(selectTargetParameters);
        Ui.Shell.ShowModalDialog(selectDialog, DialogType.Small, InteractionState.SelectTarget);

        var positiveGroup = selectDialog.Selection.Select(x => (Card) x).ToList();
        var negativeGroup = D._p.Cards.Where(x => !positiveGroup.Contains(x)).ToList();

        if (positiveGroup.Count > 1)
        {
          var orderDialogPositive = Ui.Dialogs.CardOrder.Create(positiveGroup, D._p.PositiveText);
          Ui.Shell.ShowModalDialog(orderDialogPositive, DialogType.Large, InteractionState.Disabled);
          positiveGroup.ShuffleInPlace(orderDialogPositive.Ordering);
        }

        if (negativeGroup.Count > 1)
        {
          var orderDialogNegative = Ui.Dialogs.CardOrder.Create(negativeGroup, D._p.NegativeText);
          Ui.Shell.ShowModalDialog(orderDialogNegative, DialogType.Large, InteractionState.Disabled);
          negativeGroup.ShuffleInPlace(orderDialogNegative.Ordering);
        }

        Result = new Split(new IEnumerable<Card>[]
          {
            positiveGroup,
            negativeGroup
          });
      }
    }
  }
}