namespace Grove.Core
{
  public interface ICardFactory
  {
    string Name { get; }    
    
    Card CreateCard(Player controller);
    Card CreateCardPreview();
  }
}