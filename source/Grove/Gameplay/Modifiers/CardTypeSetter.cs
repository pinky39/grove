namespace Grove.Gameplay.Modifiers
{
  using System;
  using Characteristics;
  using Infrastructure;

  [Serializable]
  public class CardTypeSetter : PropertyModifier<CardType>
  {
    private readonly CardType _type;

    private CardTypeSetter() {}

    public CardTypeSetter(CardType type)
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