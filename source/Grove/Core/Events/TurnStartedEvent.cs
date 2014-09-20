namespace Grove.Events
{
  public class TurnStartedEvent
  {
    public TurnStartedEvent(int turnCount)
    {
      TurnCount = turnCount;
    }

    public readonly int TurnCount;
  }
}