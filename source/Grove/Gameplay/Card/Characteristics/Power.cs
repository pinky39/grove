namespace Grove.Gameplay.Card.Characteristics
{
  using Core;
  using Infrastructure;
  using Modifiers;

  public class Power : Characteristic<int?>, IModifiable
  {
    private Power() {}

    public Power(int? value) : base(value) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}