namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using Decisions.Results;

  public abstract class MachinePlayRule : GameObject
  {
    public abstract IList<Playable> Process(IEnumerable<Playable> playables, ActivationPrerequisites prerequisites);

    public virtual void Initialize(Game game)
    {
      Game = game;
    }
  }
}