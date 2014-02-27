namespace Grove.Effects
{
  public class EachPlayerPutTopCardsFromLibraryToGraveyard : Effect
  {
    private readonly int _count;

    private EachPlayerPutTopCardsFromLibraryToGraveyard() {}

    public EachPlayerPutTopCardsFromLibraryToGraveyard(int count)
    {
      _count = count;
    }

    protected override void ResolveEffect()
    {
      Players.Active.Mill(_count);
      Players.Passive.Mill(_count);
    }
  }
}