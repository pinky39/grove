namespace Grove.Gameplay.Modifiers
{
  using System;
  using Infrastructure;
  using Messages;

  [Serializable]
  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {
    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}