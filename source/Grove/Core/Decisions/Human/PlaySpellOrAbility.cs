namespace Grove.Core.Decisions.Human
{
  using Grove.Ui;
  using Grove.Ui.Priority;
  using Grove.Ui.Shell;
  using Grove.Core.Zones;
  using Results;

  public class PlaySpellOrAbility : Decisions.PlaySpellOrAbility
  {
    public Configuration Configuration { get; set; }
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }
    public Stack Stack { get { return Game.Stack; } }
    public TurnInfo Turn { get { return Game.Turn; } }

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