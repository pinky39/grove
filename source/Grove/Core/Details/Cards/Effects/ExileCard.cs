namespace Grove.Core.Details.Cards.Effects
{
  public class ExileCard : Effect
  {
    public Card Card { get; set; }
    protected override void ResolveEffect()
    {
      Card.Exile();
    }
  }
}