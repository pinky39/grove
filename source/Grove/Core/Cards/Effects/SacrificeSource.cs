namespace Grove.Core.Cards.Effects
{
  public class SacrificeSource : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Sacrifice();
    }
  }
}