namespace Grove.Core.Cards.Effects
{
  public class ExileOwner : Effect
  {    
    protected override void ResolveEffect()
    {
      Source.OwningCard.Exile();
    }
  }
}