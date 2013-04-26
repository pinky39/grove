namespace Grove.Gameplay.Card.Abilities
{
  using Infrastructure;
  using Targeting;

  [Copyable]
  public class ActivationParameters
  {
    public bool PayCost = true;
    public int Repeat = 1;
    public bool SkipStack;
    public Targets Targets = new Targets();
    public int? X;
  }
}