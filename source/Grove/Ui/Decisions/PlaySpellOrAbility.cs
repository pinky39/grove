namespace Grove.Ui.Decisions
{
  using Grove.Gameplay.Decisions.Results;
  using Grove.Ui.Priority;
  using Grove.Ui.Shell;

  public class PlaySpellOrAbility : Gameplay.Decisions.PlaySpellOrAbility
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