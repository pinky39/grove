namespace Grove.Events
{
  public class EmblemRemovedEvent
  {
    public readonly Emblem Emblem;

    public EmblemRemovedEvent(Emblem emblem)
    {
      Emblem = emblem;
    }
  }
}