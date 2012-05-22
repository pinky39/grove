namespace Grove.Core
{
  using Infrastructure;
  using Modifiers;

  public class CardColors : Characteristic<ManaColors>, IModifiable
  {
    private CardColors() {}

    public CardColors(ManaColors value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}