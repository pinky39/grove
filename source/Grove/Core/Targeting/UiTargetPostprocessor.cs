namespace Grove.Core.Targeting
{
  using Ui.Shell;

  public abstract class UiTargetPostprocessor
  {
    public IShell Shell { get; set; }    

    public abstract void Postprocess(Targets targets);    
  }
}