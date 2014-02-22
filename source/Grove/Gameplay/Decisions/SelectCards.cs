namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Modifiers;
  using UserInterface;
  using UserInterface.SelectTarget;

  public class SelectCards : Decision
  {
    private readonly Params _p = new Params();

    private SelectCards() {}

    public SelectCards(Player controller, Action<Params> setParameters)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<SelectCards, ChosenCards>
    {
      private List<Card> _validCards;

      protected List<Card> ValidTargets
      {
        get
        {
          return
            _validCards ?? (_validCards =
              GenerateTargets((zone, owner) =>
                {
                  if (D._p.CanSelectOnlyCardsControlledByDecisionController && owner != D.Controller)
                    return false;

                  return zone == D._p.Zone;
                })
                .Where(x => x.IsCard())
                .Select(x => x.Card())
                .Where(IsValidCard).ToList());
        }
      }

      protected override bool ShouldExecuteQuery
      {
        get
        {
          if (D._p.MaxCount == 0)
            return false;

          return true;
        }
      }


      public override void ProcessResults()
      {
        D._p.ProcessDecisionResults.ProcessResults(Result);
      }

      private bool IsValidCard(Card card)
      {
        return D._p.Validator.IsValidCard(card);
      }

      protected override void SetResultNoQuery()
      {
        Result = new ChosenCards(ValidTargets.Take(D._p.MinCount));
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
        Result = D._p.ChooseDecisionResults.ChooseResult(ValidTargets);
      }
    }

    [Copyable]
    public class Params
    {
      public bool AurasNeedTarget;
      public bool CanSelectOnlyCardsControlledByDecisionController = true;
      public IChooseDecisionResults<List<Card>, ChosenCards> ChooseDecisionResults;
      public string Instructions;
      public int? MaxCount;
      public int MinCount;
      public Card OwningCard;
      public IProcessDecisionResults<ChosenCards> ProcessDecisionResults;
      public string Text;
      public ICardValidator Validator;
      public Zone Zone;

      public void SetValidator(Func<Card, bool> validator)
      {
        Validator = new DelegateCardValidator(validator);
      }
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
        ChosenCards chosenCards = null;

        while (true)
        {
          chosenCards = new ChosenCards();

          var validatorParameters = new TargetValidatorParameters
            {
              IsValidTarget = p => D._p.Validator.IsValidCard(p.Target.Card()),
              IsValidZone = p => p.Zone == D._p.Zone && p.ZoneOwner == D.Controller,
              MinCount = D._p.MinCount,
              MaxCount = D._p.MaxCount == null ? null : (Value) D._p.MaxCount.Value,
              Message = D._p.Text,
            };

          var validator = new TargetValidator(validatorParameters);
          validator.Initialize(Game, D.Controller, D._p.OwningCard);

          var selectTargetParameters = new SelectTargetParameters
            {
              Validator = validator,
              Instructions = D._p.Instructions
            };

          var dialog = Ui.Dialogs.SelectTarget.Create(selectTargetParameters);
          Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

          foreach (Card card in dialog.Selection)
          {
            chosenCards.Add(card);

            if (card.Is().Aura && D._p.AurasNeedTarget)
            {
              var auraTarget = SelectAuraTarget(card);

              if (auraTarget == null)
                continue;

              chosenCards.Add(auraTarget);
            }
          }

          break;
        }

        Result = chosenCards;
      }

      private Card SelectAuraTarget(Card card)
      {
        var validatorParameters = new TargetValidatorParameters
          {
            IsValidTarget = p => card.CanTarget(p.Target),
            IsValidZone = p => p.Zone == Zone.Battlefield,
            Message = "Select aura target.",
          };

        var validator = new TargetValidator(validatorParameters);
        validator.Initialize(Game, D.Controller, D._p.OwningCard);

        var selectTargetParameters = new SelectTargetParameters
          {
            Validator = validator,
            CanCancel = true,
            Instructions = D._p.Instructions
          };

        var dialog = Ui.Dialogs.SelectTarget.Create(selectTargetParameters);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

        if (dialog.WasCanceled)
          return null;

        return dialog.Selection[0].Card();
      }
    }
  }
}