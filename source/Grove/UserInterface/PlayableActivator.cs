namespace Grove.UserInterface
{
  using System;
  using Gameplay.Abilities;
  using Gameplay.Decisions.Results;

  public class PlayableActivator
  {
    public Func<ActivationParameters, Playable> GetPlayable;
    public ActivationPrerequisites Prerequisites;
  }
}