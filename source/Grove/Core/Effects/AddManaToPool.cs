namespace Grove.Core.Effects
{
  public class AddManaToPool : Effect
  {
    public ManaAmount Mana { get; set; }

    public override void Resolve()
    {
      Controller.AddManaToManaPool(Mana);
    }    
  }
}