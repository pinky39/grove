namespace Grove.Gameplay.Zones
{
  using System;
  using Effects;

  public class StackChangedEventArgs : EventArgs
  {
    public StackChangedEventArgs(Effect effect)
    {
      Effect = effect;
    }

    public Effect Effect { get; private set; }
  }
}