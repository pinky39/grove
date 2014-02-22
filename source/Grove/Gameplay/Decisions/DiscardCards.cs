namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Linq;
  using AI;
  using UserInterface;
  using UserInterface.SelectTarget;

  public class DiscardCards : Decision
  {
    private readonly Params _p = new Params();

    private DiscardCards() {}

    public DiscardCards(Player controller, Action<Params> setParameters)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<DiscardCards, ChosenCards>
    {
      protected Player CardsOwner
      {
        get
        {
          return D._p.DiscardOpponentsCards
            ? D.Controller.Opponent
            : D.Controller;
        }
      }

      protected override bool ShouldExecuteQuery
      {
        get
        {
          return D._p.Count > 0 &&
            CardsOwner.Hand.Where(D._p.Filter).Count() > D._p.Count;
        }
      }

      public override void ProcessResults()
      {
        foreach (var card in Result)
        {
          card.Discard();
        }
      }

      protected override void SetResultNoQuery()
      {
        Result = new ChosenCards(CardsOwner.Hand.Where(D._p.Filter).Take(D._p.Count));
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new ChosenCards();
      }

      protected override void ExecuteQuery()
      {
        var cardsToDiscard = CardsOwner.Hand
          .Where(D._p.Filter)
          .Select(
            card =>
              new
                {
                  Card = card,
                  Score = ScoreCalculator.CalculateDiscardScore(card, Game.Ai.IsSearchInProgress)
                });

        cardsToDiscard = D._p.DiscardOpponentsCards
          ? cardsToDiscard.OrderByDescending(x => x.Score)
          : cardsToDiscard.OrderBy(x => x.Score);

        Result = cardsToDiscard
          .Take(D._p.Count)
          .Select(x => x.Card)
          .ToList();
      }
    }

    public class Params
    {
      public int Count;
      public bool DiscardOpponentsCards;
      public Func<Card, bool> Filter = delegate { return true; };
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenCards) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var parameters = new TargetValidatorParameters
          {
            MinCount = D._p.Count,
            MaxCount = D._p.Count,
            Message = String.Format("Select {0} card(s) to discard.", D._p.Count),
            IsValidTarget = p => D._p.Filter(p.Target.Card()),
            IsValidZone = p => p.ZoneOwner == CardsOwner && p.Zone == Zone.Hand
          };

        var targetValidator = new TargetValidator(parameters);
        targetValidator.Initialize(Game, D.Controller);

        var dialog = Ui.Dialogs.SelectTarget.Create(new SelectTargetParameters
          {
            CanCancel = false,
            Validator = targetValidator
          });

        Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
        Result = dialog.Selection.ToList();
      }
    }
  }
}