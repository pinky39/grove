namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class PlayerCastsCreatureLifetime : Lifetime, IReceive<SpellPutOnStackEvent>
  {
    public void Receive(SpellPutOnStackEvent message)
    {
      if (message.Card.Is().Creature)
      {
        End();
      }
    }
  }
}