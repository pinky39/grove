namespace Grove.UserInterface
{
  using System;
  using Gameplay;
  using Gameplay.Decisions;

  public class PlayableActivator
  {
    public Func<ActivationParameters, Playable> GetPlayable;
    public ActivationPrerequisites Prerequisites;
  }
}