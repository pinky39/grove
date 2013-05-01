namespace Grove.Gameplay.Modifiers
{
  using System;
  using Infrastructure;
  using Messages;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {
    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}