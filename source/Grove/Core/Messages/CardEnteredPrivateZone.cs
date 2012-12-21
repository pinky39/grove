namespace Grove.Core.Messages
{
  public class CardEnteredBattlefield
  {
    public CardEnteredBattlefield(Player battlefieldOwner, Card card)
    {
      BattlefieldOwner = battlefieldOwner;
      Card = card;
    }

    public Player BattlefieldOwner { get; private set; }
    public Card Card { get; private set; }
  }
}