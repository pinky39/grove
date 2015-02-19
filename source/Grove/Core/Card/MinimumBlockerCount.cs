namespace Grove
{
  using Modifiers;

  public class MinimumBlockerCount : Characteristic<int?>, IAcceptsCardModifier
  {
    private MinimumBlockerCount() {}

    public MinimumBlockerCount(int value)
      : base(value) {}

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}
