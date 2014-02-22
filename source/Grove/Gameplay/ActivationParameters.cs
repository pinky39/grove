namespace Grove.Gameplay
{
  using System;
  using Grove.Infrastructure;

  [Copyable, Serializable]
  public class ActivationParameters
  {
    public bool PayCost = true;
    public int Repeat = 1;
    public bool SkipStack;
    public Targets Targets = new Targets();
    public int? X;
  }
}