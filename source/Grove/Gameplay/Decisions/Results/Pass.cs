namespace Grove.Gameplay.Decisions.Results
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