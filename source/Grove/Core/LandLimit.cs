namespace Grove.Core
{
  using Cards;
  using Cards.Modifiers;
  using Infrastructure;

  public class LandLimit : Characteristic<int?>, IModifiable
  {
    private LandLimit() {}

    public LandLimit(int value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}