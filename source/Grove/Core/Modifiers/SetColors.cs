namespace Grove.Modifiers
{
  using System.Collections.Generic;

  public class SetColors : Modifier, ICardModifier
  {
    private readonly List<CardColor> _colors = new List<CardColor>();
    private CardColorSetter _cardColorSetter;
    private ColorsOfCard _colorsOfCard;

    private SetColors() {}

    public SetColors(CardColor color)
    {
      _colors.Add(color);
    }

    public override void Apply(ColorsOfCard colors)
    {
      _colorsOfCard = colors;
      _cardColorSetter = new CardColorSetter(_colors);
      _cardColorSetter.Initialize(ChangeTracker);
      _colorsOfCard.AddModifier(_cardColorSetter);
    }

    protected override void Unapply()
    {
      _colorsOfCard.RemoveModifier(_cardColorSetter);
    }
  }
}