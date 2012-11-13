namespace Grove.Core.Cards.Effects
{
  public class ExileCard : Effect
  {
    public Card Card;
    
    protected override void ResolveEffect()
    {
      Card.Exile();
    }
  }
}