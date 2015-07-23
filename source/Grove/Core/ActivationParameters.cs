namespace Grove
{
  using System;
  using Infrastructure;

  [Copyable, Serializable]
  public class ActivationParameters
  {
    public bool PayManaCost = true;
    public int Repeat = 1;
    public bool SkipStack;
    public Targets Targets = new Targets();
    public int? X;
  }
}