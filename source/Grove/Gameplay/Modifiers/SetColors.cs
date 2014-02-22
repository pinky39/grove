namespace Grove.Gameplay.Modifiers
{
  using System.Collections.Generic;

  public class SetColors : Modifier, ICardModifier
  {
    private readonly List<CardColor> _colors = new List<CardColor>();
    private CardColorSetter _cardColorSetter;
    private CardColors _cardColors;

    private SetColors() {}

    public SetColors(CardColor color)
    {
      _colors.Add(color);
    }

    public override void Apply(CardColors colors)
    {
      _cardColors = colors;
      _cardColorSetter = new CardColorSetter(_colors);
      _cardColorSetter.Initialize(ChangeTracker);
      _cardColors.AddModifier(_cardColorSetter);
    }

    protected override void Unapply()
    {
      _cardColors.RemoveModifier(_cardColorSetter);
    }
  }
}