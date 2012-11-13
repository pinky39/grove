namespace Grove.Core.Decisions.Human
{
  using Grove.Ui;
  using Grove.Ui.SelectEffectChoice;
  using Grove.Ui.Shell;

  public class ChooseEffectOptions : Decisions.ChooseEffectOptions
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