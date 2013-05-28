namespace Grove.Gameplay.Characteristics
{
  using System;
  using Infrastructure;
  using Modifiers;

  [Serializable]
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