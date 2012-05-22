namespace Grove.Core.Effects
{
  public class Regenerate : Effect
  {    
    public override void Resolve()
    {
      Source.OwningCard.CanRegenerate = true;
    }    
  }
}