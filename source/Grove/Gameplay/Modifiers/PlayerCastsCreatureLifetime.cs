namespace Grove.Gameplay.Modifiers
{
  using Infrastructure;
  using Messages;

  public class PlayerCastsCreatureLifetime : Lifetime, IReceive<AfterSpellWasPutOnStack>
  {
    public void Receive(AfterSpellWasPutOnStack message)
    {
      if (message.Card.Is().Creature)
      {
        End();
      }
    }
  }
}