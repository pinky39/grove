namespace Grove.Events
{
  public class SpellPutOnStackEvent
  {
    public readonly Effect Effect;
    public Card Card { get { return Effect.Source.OwningCard; } }
    public Player Controller { get { return Effect.Controller; } }

    public SpellPutOnStackEvent(Effect effect)
    {
      Effect = effect;
    }
  }
}