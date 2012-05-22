namespace Grove.Core.Controllers.Human
{
  using Ui;
  using Ui.Priority;
  using Ui.Shell;
  using Zones;

  public class PlaySpellOrAbility : Controllers.PlaySpellOrAbility
  {
    public Configuration Configuration { get; set; }
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }
    public Stack Stack { get; set; }
    public TurnInfo Turn { get; set; }

    protected override void ExecuteQuery()
    {
      if (Configuration.ShouldAutoPass(Turn.Step, Player.IsActive) && Stack.IsEmpty)
      {
        Result = new Results.Pass();
        return;
      }

      var dialog = DialogFactory.Create();
      Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.Play);
      Result = dialog.Playable;
    }
  }
}