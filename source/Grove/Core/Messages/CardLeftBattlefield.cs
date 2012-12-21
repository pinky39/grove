namespace Grove.Core.Messages
{
  public class CardLeftBattlefield
  {
    public CardLeftBattlefield(Player battlefieldOwner, Card card)
    {
      BattlefieldOwner = battlefieldOwner;
      Card = card;
    }

    public Player BattlefieldOwner { get; private set; }
    public Card Card { get; private set; }
  }
}