namespace Grove.Gameplay.Modifiers
{
  using System;
  using Infrastructure;
  using Messages;

  [Serializable]
  public class PlayerCastsCreatureLifetime : Lifetime, IReceive<PlayerHasCastASpell>
  {
    public void Receive(PlayerHasCastASpell message)
    {
      if (message.Card.Is().Creature)
      {
        End();
      }
    }
  }
}