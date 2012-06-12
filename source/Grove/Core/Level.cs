namespace Grove.Core
{
  using Infrastructure;
  using Modifiers;

  public class Level : Characteristic<int?>, IModifiable
  {
    private Level() {}

    public Level(int? value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
      : base(value, changeTracker, hashDependancy) {}
    
    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}