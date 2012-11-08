namespace Grove.Core
{
  public interface ICardFactory
  {
    string Name { get; }

    Card CreateCard(Player owner, Game game);
    Card CreateCardPreview();
  }
}