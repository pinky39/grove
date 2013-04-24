namespace Grove.Core.Decisions.Human
{
  using Results;
  using Ui;
  using Ui.Priority;
  using Ui.Shell;

  public class PlaySpellOrAbility : Decisions.PlaySpellOrAbility
  {
    public Configuration Configuration { get; set; }
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      if (Configuration.ShouldAutoPass(Turn.Step, Controller.IsActive) && Stack.IsEmpty)
      {
        Result = new Pass();
        return;
      }

      var dialog = DialogFactory.Create();
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.PlaySpellsOrAbilities);
      Result = dialog.Playable ?? new Pass();
    }
  }
}