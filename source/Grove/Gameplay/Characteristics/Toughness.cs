namespace Grove.Gameplay.Characteristics
{
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Toughness : Characteristic<int?>, IAcceptsCardModifier
  {
    private Toughness() {}

    public Toughness(int? value) : base(value) {}

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}