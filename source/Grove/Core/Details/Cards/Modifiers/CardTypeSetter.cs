namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;

  public class CardTypeSetter : PropertyModifier<CardType>
  {
    private readonly CardType _type;

    private CardTypeSetter() : base(null) {}

    public CardTypeSetter(CardType type, ChangeTracker changeTracker) : base(changeTracker)
    {
      _type = type;
    }

    public override int Priority { get { return 2; } }

    public override CardType Apply(CardType before)
    {
      return _type;
    }
  }
}