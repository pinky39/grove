namespace Grove.Core.Cards
{
  using Grove.Infrastructure;
  using Modifiers;

  [Copyable]
  public class Toughness : Characteristic<int?>, IModifiable
  {
    private Toughness() {}

    public Toughness(int? value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}