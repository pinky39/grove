namespace Grove.Core.Modifiers
{
  using Targeting;

  public class ModifierParameters
  {
    public Card Source;
    public ITarget Target;
    public int? X;
    public bool EndOfTurn;
    public int? MinLevel;
    public int? MaxLevel;
  }
}