namespace Grove.Artifical.DraftAlgorithms
{
  public interface IDraftingStrategies
  {
    Forcing CreateForcingStrategy(CardRatings ratings);
  }
}