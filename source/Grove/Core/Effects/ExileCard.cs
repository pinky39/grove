namespace Grove.Effects
{
  public class ExileCard : Effect
  {
    private readonly DynParam<Card> _card;
    private readonly Zone _from;

    private ExileCard() {}

    public ExileCard(DynParam<Card> card, Zone from)
    {
      _card = card;
      _from = from;
      RegisterDynamicParameters(card);
    }

    protected override void ResolveEffect()
    {
      _card.Value.ExileFrom(_from);
    }
  }
}