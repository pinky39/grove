namespace Grove.Modifiers
{
  public class IntegerSetter : IntegerModifier
  {
    public IntegerSetter() {}

    public IntegerSetter(int? value) : base(value) {}

    public override int Priority { get { return 1; } }

    public override int? Apply(int? before)
    {
      return Value;
    }
  }
}