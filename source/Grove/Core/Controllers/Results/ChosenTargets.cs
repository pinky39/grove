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
  }
}