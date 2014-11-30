namespace Grove.Effects
{
  public class LookAtTopCardsPutOneInHandOthersIntoGraveyard : LookAtTopCardsPutOneInHandOthersIntoZone
  {
    private LookAtTopCardsPutOneInHandOthersIntoGraveyard() {}

    public LookAtTopCardsPutOneInHandOthersIntoGraveyard(int count) : base(count) { }

    protected override void PutCardIntoZone(Card card)
    {
      Controller.PutCardToGraveyard(card);
    }
  }
}
