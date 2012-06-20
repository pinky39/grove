namespace Grove.Core.Effects
{
  public class AddManaToPool : Effect
  {
    public IManaAmount Mana { get; set; }

    protected override void ResolveEffect()
    {
      Controller.AddManaToManaPool(Mana);
    }    
  }
}