namespace Grove.Core.Effects
{
  public class ExileCard : Effect
  {
    private readonly DynParam<Card> _card;

    private ExileCard() {}

    public ExileCard(DynParam<Card> card)
    {
      _card = card;
      RegisterDynamicParameters(card);
    }

    protected override void ResolveEffect()
    {
      _card.Value.Exile();
    }
  }
}