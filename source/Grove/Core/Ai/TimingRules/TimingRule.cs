namespace Grove.Core.Ai.TimingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions.Results;

  public abstract class TimingRule : MachinePlayRule
  {
    public override IList<Playable> Process(IEnumerable<Playable> playables, ActivationPrerequisites prerequisites)
    {
      return playables.Where(x => ShouldPlay(x, prerequisites)).ToArray();
    }

    public abstract bool ShouldPlay(Playable playable, ActivationPrerequisites prerequisites);
  }
}