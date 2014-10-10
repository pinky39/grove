namespace Grove.Events
{
  public class PlayerDiscardsCardEvent
  {
    public readonly Player Player;
    public readonly Card Card;

    public PlayerDiscardsCardEvent(Player player, Card card)
    {
      Player = player;
      Card = card;
    }
  }
}
