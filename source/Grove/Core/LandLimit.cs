namespace Grove.Core
{
  using Infrastructure;
  using Modifiers;

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