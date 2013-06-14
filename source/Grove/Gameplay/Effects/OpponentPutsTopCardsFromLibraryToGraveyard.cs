namespace Grove.Gameplay.Effects
{
  public class OpponentPutsTopCardsFromLibraryToGraveyard : Effect
  {
    private readonly int _count;

    private OpponentPutsTopCardsFromLibraryToGraveyard() {}

    public OpponentPutsTopCardsFromLibraryToGraveyard(int count)
    {
      _count = count;
    }

    protected override void ResolveEffect()
    {
      Controller.Opponent.Mill(_count);
    }
  }
}