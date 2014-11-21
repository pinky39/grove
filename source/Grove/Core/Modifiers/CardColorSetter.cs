namespace Grove.Modifiers
{
  using System.Collections.Generic;

  public class CardColorSetter : PropertyModifier<List<CardColor>>
  {
    private readonly List<CardColor> _colors;

    private CardColorSetter() {}

    public CardColorSetter(List<CardColor> colors)
    {
      _colors = colors;
    }

    public override int Priority
    {
      get { return 1; }
    }

    public override List<CardColor> Apply(List<CardColor> before)
    {
      return _colors;
    }
  }
}