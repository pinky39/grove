namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;
  using Messages;

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