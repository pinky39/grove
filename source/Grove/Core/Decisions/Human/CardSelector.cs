namespace Grove.Core.Decisions.Human
{
  using Results;
  using Targeting;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class CardSelector
  {
    public ViewModel.IFactory TargetDialog { get; set; }
    public IShell Shell { get; set; }
    public Game Game { get; set; }

    public void ExecuteQuery(SelectCards selectCards)
    {
      var chosenCards = new ChosenCards();

      var validatorParameters = new TargetValidatorParameters
        {
          IsValidTarget = p => selectCards.IsValidCard(p.Target.Card()),
          IsValidZone = p => p.Zone == selectCards.Zone && p.ZoneOwner == selectCards.Controller,
          MinCount = selectCards.MinCount,
          MaxCount = selectCards.MaxCount,
          Message = selectCards.Text,          
        };

      var validator = new TargetValidator(validatorParameters);
      validator.Initialize(Game, selectCards.Controller, selectCards.OwningCard);

      var selectTargetParameters = new SelectTargetParameters
        {
          Validator = validator,
          CanCancel = false,
        };
      
      var dialog = TargetDialog.Create(selectTargetParameters);
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

      foreach (var target in dialog.Selection)
      {
        chosenCards.Add(target.Card());
      }

      selectCards.Result = chosenCards;
    }
  }
}