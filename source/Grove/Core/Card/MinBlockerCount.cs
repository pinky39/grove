namespace Grove
{
  using Modifiers;

  public class MinBlockerCount : Characteristic<int?>, IAcceptsCardModifier
  {
    private MinBlockerCount() {}

    public MinBlockerCount(int value)
      : base(value) {}

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}
