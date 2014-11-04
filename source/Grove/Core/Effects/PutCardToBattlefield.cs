namespace Grove.Effects
{
  public class PutCardToBattlefield : Effect
  {
    private readonly DynParam<Card> _card;
    private readonly Zone _from;

    private PutCardToBattlefield() {}

    public PutCardToBattlefield(DynParam<Card> card, Zone from)
    {
      _card = card;
      _from = from;

      RegisterDynamicParameters(card);
    }

    public override bool CanBeResolved()
    {
      return _card.Value.Zone == _from && base.CanBeResolved();
    }

    protected override void ResolveEffect()
    {
      _card.Value.PutToBattlefieldFrom(_from);
    }
  }
}