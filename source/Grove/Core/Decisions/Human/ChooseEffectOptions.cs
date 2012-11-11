namespace Grove.Core.Controllers.Human
{
  using Ui;
  using Ui.SelectEffectChoice;
  using Ui.Shell;

  public class ChooseEffectOptions : Controllers.ChooseEffectOptions
  {
    public ViewModel.IFactory EffectChoiceDialog { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var dialog = EffectChoiceDialog.Create(Choices, Text);
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

      Result = dialog.ChosenOptions;
    }
  }
}