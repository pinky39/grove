namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using Targeting;

  [Serializable]
  public class ChosenTargets
  {
    public ChosenTargets(Targets targets)
    {
      Targets = targets;
    }

    public Targets Targets { get; private set; }

    public bool HasTargets { get { return Targets != null && Targets.Count > 0; } }
  }
}