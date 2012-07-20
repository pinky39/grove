namespace Grove.Core.Details.Cards.Effects
{
  public class PutIntoPlay : Effect
  {
    public bool PutIntoPlayTapped;

    protected override void ResolveEffect()
    {                  
      if (PutIntoPlayTapped)
      {
        Source.OwningCard.Tap();
      }
    }
  }
}