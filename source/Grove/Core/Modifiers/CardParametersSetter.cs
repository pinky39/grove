namespace Grove.Modifiers
{
  public class CardParametersSetter : PropertyModifier<CardParameters>
  {
    private readonly CardParameters _cardParameters;

    private CardParametersSetter() {}

    public CardParametersSetter(CardParameters cardParameters)
    {
      _cardParameters = cardParameters;
    }

    public override int Priority
    {
      get { return 1; }
    }

    public override CardParameters Apply(CardParameters before)
    {
      return _cardParameters;
    }
  }
}