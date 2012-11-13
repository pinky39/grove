namespace Grove.Core.Cards
{
  using Grove.Infrastructure;
  using Modifiers;

  public class Power : Characteristic<int?>, IModifiable
  {
    private Power() {}

    public Power(int? value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}