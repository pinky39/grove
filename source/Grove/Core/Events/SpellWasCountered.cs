namespace Grove.Events
{
  using Grove.Effects;

  public class SpellWasCountered
  {
    public Card Card { get; set; }
    public SpellCounterReason Reason { get; set; }
  }
}