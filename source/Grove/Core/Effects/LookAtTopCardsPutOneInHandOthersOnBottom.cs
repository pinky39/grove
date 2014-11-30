namespace Grove.Effects
{
  public class LookAtTopCardsPutOneInHandOthersOnBottom : LookAtTopCardsPutOneInHandOthersIntoZone
  {
    private LookAtTopCardsPutOneInHandOthersOnBottom() {}

    public LookAtTopCardsPutOneInHandOthersOnBottom(int count) : base(count) {}

    protected override void PutCardIntoZone(Card card)
    {
      Controller.PutOnBottomOfLibrary(card);
    }
  }
}