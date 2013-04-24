﻿namespace Grove.Gameplay.Modifiers
{
  using Grove.Infrastructure;
  using Messages;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {
    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}