namespace Grove.Gameplay.Card.Abilities
{
  using Grove.Infrastructure;
  using Targeting;

  [Copyable]
  public class ActivationParameters
  {
    public Targets Targets = new Targets();
    public bool PayCost = true;
    public bool SkipStack;
    public int Repeat = 1;
    public int? X;    
  }
}