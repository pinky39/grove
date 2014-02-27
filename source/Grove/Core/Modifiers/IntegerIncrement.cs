namespace Grove.Modifiers
{
  public class IntegerIncrement : IntegerModifier
  {
    public IntegerIncrement() {}
    public IntegerIncrement(int? value) : base(value) {}
    public override int Priority { get { return 2; } }

    public override int? Apply(int? before)
    {
      return before + Value;
    }

    public static IntegerIncrement operator ++(IntegerIncrement integerIncrement)
    {
      integerIncrement.Value++;
      return integerIncrement;
    }

    public static IntegerIncrement operator --(IntegerIncrement integerIncrement)
    {
      integerIncrement.Value--;
      return integerIncrement;
    }
  }
}