namespace Grove.Core.Zones
{
  using System;
  using Effects;

  public class StackChangedEventArgs : EventArgs
  {
    public Effect Effect { get; private set; }
    
    public StackChangedEventArgs(Effect effect)
    {
      Effect = effect;
    }
  }
}