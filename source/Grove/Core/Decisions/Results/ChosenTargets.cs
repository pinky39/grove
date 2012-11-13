namespace Grove.Core.Decisions.Results
{
  using Grove.Core.Targeting;

  public class ChosenTargets
  {
    public ChosenTargets(Targets targets)
    {
      Targets = targets;
    }

    public Targets Targets { get; private set; }

    public bool HasTargets
    {
      get { return Targets != null && Targets.Count > 0; }
    }
  }
}