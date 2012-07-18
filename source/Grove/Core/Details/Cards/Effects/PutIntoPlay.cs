namespace Grove.Core.Details.Cards.Effects
{
  public class PutIntoPlay : Effect
  {
    public bool PutIntoPlayTapped;

    protected override void ResolveEffect()
    {
      Controller.PutCardIntoPlay(Source.OwningCard);

      if (PutIntoPlayTapped)
      {
        Source.OwningCard.Tap();
      }
    }
  }
}