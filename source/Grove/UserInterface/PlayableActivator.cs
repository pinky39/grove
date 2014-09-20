namespace Grove.UserInterface
{
  using System;
  using Decisions;

  public class PlayableActivator
  {
    public Func<ActivationParameters, Playable> GetPlayable;
    public ActivationPrerequisites Prerequisites;
  }
}