namespace Grove.Gameplay.Characteristics
{
  using Infrastructure;
  using Modifiers;

  public class LandLimit : Characteristic<int?>, IAcceptsPlayerModifier
  {
    private LandLimit() {}

    public LandLimit(int value)
      : base(value) {}

    public void Accept(IPlayerModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}