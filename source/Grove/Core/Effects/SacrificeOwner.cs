namespace Grove.Effects
{
  public class SacrificeOwner : Effect
  {
    protected override void ResolveEffect()
    {                  
      Source.OwningCard.Sacrifice();
    }
  }
}