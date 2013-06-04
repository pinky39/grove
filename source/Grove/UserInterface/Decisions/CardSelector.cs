namespace Grove.UserInterface.Decisions
{
  using Gameplay.Decisions.Results;
  using Gameplay.Modifiers;
  using Gameplay.Targeting;
  using SelectTarget;

  public class CardSelector : ViewModelBase
  {
    public void ExecuteQuery(SelectCards selectCards)
    {
      var chosenCards = new ChosenCards();

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
          CanCancel = false,
          Instructions = selectCards.Instructions
        };

      var dialog = ViewModels.SelectTarget.Create(selectTargetParameters);
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

      foreach (var target in dialog.Selection)
      {
        chosenCards.Add(target.Card());
      }

      selectCards.Result = chosenCards;
    }
  }
}