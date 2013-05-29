namespace Grove.Gameplay.Effects
{
  public class ExileOwner : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Exile();
    }
  }
}