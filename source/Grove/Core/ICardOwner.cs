namespace Grove.Core
{
  public interface ICardOwner : IPlayer
  {
    void PutCardOnTopOfLibrary(Card card);
    void PutCardToHand(Card card);
    void PutCardToExile(Card card);
    void ShuffleIntoLibrary(Card card);
    void PutCardToGraveyard(Card card);
    void DiscardCard(Card card);    
  }
}