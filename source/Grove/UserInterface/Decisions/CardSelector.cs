namespace Grove.UserInterface.Decisions
{
  using Gameplay;
  using Gameplay.Decisions.Results;
  using Gameplay.Modifiers;
  using Gameplay.Targeting;
  using Gameplay.Zones;
  using SelectTarget;

  public class CardSelector : ViewModelBase
  {
    public void ExecuteQuery(SelectCards selectCards)
    {
      ChosenCards chosenCards = null;

      while (true)
      {
        chosenCards = new ChosenCards();

        var validatorParameters = new TargetValidatorParameters
          {
            IsValidTarget = p => selectCards.IsValidCard(p.Target.Card()),
            IsValidZone = p => p.Zone == selectCards.Zone && p.ZoneOwner == selectCards.Controller,
            MinCount = selectCards.MinCount,
            MaxCount = selectCards.MaxCount == null ? null : (Value) selectCards.MaxCount.Value,
            Message = selectCards.Text,
          };

        var validator = new TargetValidator(validatorParameters);
        validator.Initialize(CurrentGame, selectCards.Controller, selectCards.OwningCard);

        var selectTargetParameters = new SelectTargetParameters
          {
            Validator = validator,
            Instructions = selectCards.Instructions
          };

        var dialog = ViewModels.SelectTarget.Create(selectTargetParameters);
        Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

        foreach (Card card in dialog.Selection)
        {
          chosenCards.Add(card);

          if (card.Is().Aura && selectCards.AurasNeedTarget)
          {
            var auraTarget = SelectAuraTarget(card, selectCards);

            if (auraTarget == null)
              continue;

            chosenCards.Add(auraTarget);
          }
        }

        break;
      }

      selectCards.Result = chosenCards;
    }

    private Card SelectAuraTarget(Card card, SelectCards selectCards)
    {
      var validatorParameters = new TargetValidatorParameters
        {
          IsValidTarget = p => card.CanTarget(p.Target),
          IsValidZone = p => p.Zone == Zone.Battlefield,
          Message = "Select aura target.",
        };

      var validator = new TargetValidator(validatorParameters);
      validator.Initialize(CurrentGame, selectCards.Controller, selectCards.OwningCard);

      var selectTargetParameters = new SelectTargetParameters
        {
          Validator = validator,
          CanCancel = true,
          Instructions = selectCards.Instructions
        };

      var dialog = ViewModels.SelectTarget.Create(selectTargetParameters);
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

      if (dialog.WasCanceled)
        return null;

      return dialog.Selection[0].Card();
    }
  }
}