namespace Grove.Ui.Decisions
{
  using SelectEffectChoice;
  using Shell;

  public class ChooseEffectOptions : Gameplay.Decisions.ChooseEffectOptions
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