namespace Grove.Core.Modifiers
{
  using Infrastructure;

  public class CardTypeAdder : PropertyModifier<CardType>
  {
    private readonly CardType _type;

    private CardTypeAdder() : base(null) {}

    public CardTypeAdder(CardType type, ChangeTracker changeTracker) : base(changeTracker)
    {
      _type = type;
    }

    public override int Priority
    {
      get { return 2; }
    }

    public override CardType Apply(CardType before)
    {
      return before + _type;
    }
  }
}