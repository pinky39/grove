namespace Grove.UserInterface.Decisions
{
  using Gameplay.Decisions.Results;
  using Priority;
  using Shell;

  public class PlaySpellOrAbility : Gameplay.Decisions.PlaySpellOrAbility
  {
    public Configuration Configuration { get; set; }
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }    
    
    protected override void ExecuteQuery()
    {
      if (Configuration.ShouldAutoPass(Turn.Step, Controller.IsActive) && Stack.IsEmpty)
      {
        Result = new ChosenPlayable {Playable = new Pass()};
        return;
      }

      var dialog = DialogFactory.Create();
      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.PlaySpellsOrAbilities);
      Result = new ChosenPlayable {Playable = dialog.Playable ?? new Pass()};
    }
  }
}