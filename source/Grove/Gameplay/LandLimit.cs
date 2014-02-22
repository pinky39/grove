namespace Grove.Gameplay
{
  using Grove.Infrastructure;
  using Grove.Gameplay.Modifiers;

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