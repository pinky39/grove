namespace Grove.Core
{
  public class TargetCandidate
  {
    public ITarget Target { get; private set; }

    public TargetCandidate(ITarget target)
    {
      Target = target;
    }
  }
}