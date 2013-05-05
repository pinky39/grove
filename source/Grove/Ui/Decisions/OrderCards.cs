namespace Grove.Ui.Decisions
{
  using CardOrder;
  using Gameplay.Decisions.Results;
  using Shell;

  public class OrderCards : Gameplay.Decisions.OrderCards
  {
    public IShell Shell { get; set; }
    public ViewModel.IFactory Dialog { get; set; }

    protected override void ExecuteQuery()
    {
      var dialog = Dialog.Create(Cards, Message);
      Shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);
      Result = new Ordering(dialog.Ordering);
    }
  }
}