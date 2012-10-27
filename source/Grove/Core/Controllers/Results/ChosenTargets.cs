namespace Grove.Core.Controllers.Results
{
  using Targeting;

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