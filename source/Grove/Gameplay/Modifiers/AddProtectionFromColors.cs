namespace Grove.Gameplay.Modifiers
{
  using System;
  using System.Collections.Generic;
  using Card.Characteristics;

  public class AddProtectionFromColors : Modifier
  {
    private readonly Func<Modifier, IEnumerable<CardColor>> _colors;
    private Protections _protections;

    private AddProtectionFromColors() {}

    public AddProtectionFromColors(Func<Modifier, IEnumerable<CardColor>> colors)
    {
      _colors = colors;
    }

    public AddProtectionFromColors(IEnumerable<CardColor> colors) : this(delegate { return colors; }) {}

    public AddProtectionFromColors(CardColor color)
      : this(delegate { return new[] {color}; }) {}

    public override void Apply(Protections protections)
    {
      _protections = protections;

      foreach (var cardColor in _colors(this))
      {
        _protections.AddProtectionFromColor(cardColor);
      }
    }

    protected override void Unapply()
    {
      foreach (var cardColor in _colors(this))
      {
        _protections.RemoveProtectionFromColor(cardColor);
      }
    }
  }
}