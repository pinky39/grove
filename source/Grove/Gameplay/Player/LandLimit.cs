namespace Grove.Gameplay.Player
{
  using Infrastructure;
  using Modifiers;

  public class LandLimit : Characteristic<int?>, IModifiable
  {
    private LandLimit() {}

    public LandLimit(int value)
      : base(value) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}