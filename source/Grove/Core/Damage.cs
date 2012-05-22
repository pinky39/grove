namespace Grove.Core
{
  using Infrastructure;

  [Copyable]
  public class Damage : IHashable
  {
    private Damage() {}

    public Damage(Card source, int amount)
    {
      Source = source;
      Amount = amount;
    }

    public int Amount { get; private set; }

    public bool IsLeathal { get { return Source.Has().Deathtouch; } }
    public Card Source { get; private set; }

    public int CalculateHash(HashCalculator hashCalculator)
    {
      return hashCalculator.Calculate(Amount);
    }
  }
}