namespace Grove.Core.Decisions.Results
{
  public class Pass : Playable
  {
    public override bool WasPriorityPassed { get { return true; } }

    public override string ToString()
    {
      return "pass";
    }
  }
}