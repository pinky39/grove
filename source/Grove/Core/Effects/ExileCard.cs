namespace Grove.Core.Effects
{
  using System;

  public class ExileCard : Effect
  {
    private readonly Func<Effect, Card> _getCard;
    
    public ExileCard(Func<Effect, Card> getCard)
    {
      _getCard = getCard;
    }

    protected override void ResolveEffect()
    {
      _getCard(this).Exile();
    }
  }
}