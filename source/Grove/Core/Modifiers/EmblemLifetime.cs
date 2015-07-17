namespace Grove.Modifiers
{
  using Events;
  using Infrastructure;

  public class EmblemLifetime : Lifetime, IReceive<EmblemRemovedEvent>
  {
    private readonly Emblem _emblem;

    private EmblemLifetime() {}

    public EmblemLifetime(Emblem emblem)
    {
      _emblem = emblem;
    }

    public void Receive(EmblemRemovedEvent e)
    {
      if (e.Emblem == _emblem)
      {
        End();
      }
    }
  }
}