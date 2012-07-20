namespace Grove.Core
{
  public interface ICardFactory
  {
    string Name { get; }

    Card CreateCard(IPlayer owner);
    Card CreateCardPreview();
  }
}