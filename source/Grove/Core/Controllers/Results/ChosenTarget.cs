namespace Grove.Core.Controllers.Results
{
  public class ChosenTarget
  {
    public ChosenTarget(ITarget target)
    {
      Target = target;
    }

    public ITarget Target { get; private set; }        
  }
}