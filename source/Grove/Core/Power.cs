namespace Grove.Core
{
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Power : Characteristic<int?>, IModifiable
  {
    private Power()
    {      
    }

    public Power(int? value, ChangeTracker changeTracker, IHashDependancy hashDependancy) 
      : base(value, changeTracker, hashDependancy) {}

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}