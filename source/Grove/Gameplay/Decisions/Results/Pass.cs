namespace Grove.Gameplay.Decisions.Results
{
  using System;

  [Serializable]
  public class Pass : Playable
  {
    public override bool WasPriorityPassed { get { return true; } }

    public override string ToString()
    {
      return "pass";
    }
  }
}