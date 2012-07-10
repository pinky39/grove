namespace Grove.Core.Effects
{
  public class SacrificeSource : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Sacrifice();
    }
  }
}