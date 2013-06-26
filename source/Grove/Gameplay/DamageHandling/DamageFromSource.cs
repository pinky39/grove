namespace Grove.Gameplay.DamageHandling
{
  using Infrastructure;

  [Copyable]
  public class DamageFromSource
  {
    private DamageFromSource() {}

    public DamageFromSource(int amount, Card source)
    {
      Amount = amount;
      Source = source;
    }

    public int Amount { get; set; }
    public Card Source { get; set; }

    public bool IsLeathal { get { return Source.Has().Deathtouch; } }
  }
}