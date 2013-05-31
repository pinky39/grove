namespace Grove.Gameplay.Decisions.Results
{
  using System;

  [Serializable]
  public class Pass : IPlayable
  {
    public bool WasPriorityPassed { get { return true; } }

    public bool CanPlay()
    {
      return true;
    }

    public void Play() {}

    public override string ToString()
    {
      return "pass";
    }
  }
}