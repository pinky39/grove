namespace Grove.Decisions
{
  using System;

  [Serializable]
  public class ChosenTargets
  {
    public ChosenTargets(Targets targets)
    {
      Targets = targets;
    }

    public Targets Targets { get; private set; }

    public bool HasTargets { get { return Targets != null && Targets.Count > 0; } }

    public static ChosenTargets None()
    {
      return new ChosenTargets(new Targets());
    }
  }
}