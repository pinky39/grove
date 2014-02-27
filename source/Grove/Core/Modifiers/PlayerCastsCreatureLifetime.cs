namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

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