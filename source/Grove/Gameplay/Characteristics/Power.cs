namespace Grove.Gameplay.Characteristics
{
  using Infrastructure;
  using Modifiers;

  public class Power : Characteristic<int?>, IAcceptsCardModifier
  {
    private Power() {}

    public Power(int? value) : base(value) {}

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}