namespace Grove.Core.Effects
{
  public class AddManaToPool : Effect
  {
    public IManaAmount Mana { get; set; }

    public override void Resolve()
    {
      Controller.AddManaToManaPool(Mana);
    }    
  }
}