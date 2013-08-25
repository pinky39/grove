namespace Grove.Gameplay.Messages
{
  using Effects;

  public class SpellWasCountered
  {
    public Card Card { get; set; }
    public SpellCounterReason Reason { get; set; }
  }
}