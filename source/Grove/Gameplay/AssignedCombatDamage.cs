namespace Grove.Gameplay
{
  using Infrastructure;

  [Copyable]
  public class AssignedCombatDamage
  {
    private AssignedCombatDamage() {}

    public AssignedCombatDamage(int amount, Card source)
    {
      Amount = amount;
      Source = source;
    }

    public int Amount { get; set; }
    public Card Source { get; set; }

    public bool IsLeathal { get { return Source.Has().Deathtouch; } }
  }
}