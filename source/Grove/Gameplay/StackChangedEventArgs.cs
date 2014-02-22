namespace Grove.Gameplay
{
  using System;
  using Grove.Gameplay.Effects;

  public class StackChangedEventArgs : EventArgs
  {
    public StackChangedEventArgs(Effect effect)
    {
      Effect = effect;
    }

    public Effect Effect { get; private set; }
  }
}