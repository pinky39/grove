namespace Grove.Effects
{
  public class LookAtTopCardsPutPartInHandRestIntoGraveyard : LookAtTopCardsPutPartInHandRestIntoZone
  {
    private LookAtTopCardsPutPartInHandRestIntoGraveyard() {}

    public LookAtTopCardsPutPartInHandRestIntoGraveyard(int count, int toHandAmount = 1) : base(count, toHandAmount) { }

    protected override void PutCardIntoZone(Card card)
    {
      Controller.PutCardToGraveyard(card);
    }
  }
}
