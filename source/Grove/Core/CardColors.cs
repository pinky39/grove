namespace Grove.Core
{
  using Mana;
  using Modifiers;

  public class CardColors : Characteristic<ManaColors>, IModifiable
  {
    private CardColors() {}

    public CardColors(ManaColors value) : base(value) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}