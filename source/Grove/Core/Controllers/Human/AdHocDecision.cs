namespace Grove.Core.Controllers.Human
{
  using Ui.SelectTarget;
  using Ui.Shell;

  public class AdhocDecision<T> : Controllers.AdhocDecision<T> where T : class
  {
    public IShell Shell { get; set; }
    public ViewModel.IFactory TargetDialog { get; set; }
    public Ui.SelectEffectChoice.ViewModel.IFactory EffectChoiceDialog { get; set; }

    protected override void ExecuteQuery()
    {
      Result = QueryUi(this);
    }
  }
}