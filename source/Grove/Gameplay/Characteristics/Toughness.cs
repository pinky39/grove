namespace Grove.Gameplay.Characteristics
{
  using System;
  using Infrastructure;
  using Modifiers;

  [Copyable, Serializable]
  public class Toughness : Characteristic<int?>, IModifiable
  {
    private Toughness() {}

    public Toughness(int? value) : base(value) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}