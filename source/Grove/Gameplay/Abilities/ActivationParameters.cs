namespace Grove.Gameplay.Abilities
{
  using System;
  using Infrastructure;
  using Targeting;

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