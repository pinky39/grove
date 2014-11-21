namespace Grove.Modifiers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class AddProtectionFromColors : Modifier, ICardModifier
  {
    private readonly Func<Modifier, IEnumerable<CardColor>> _colors;
    private Protections _protections;
    private AddToList<CardColor> _modifier;

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

      var colors = _colors(this).ToList();

      _modifier = new AddToList<CardColor>(colors);
      _modifier.Initialize(ChangeTracker);
      protections.AddModifier(_modifier);
    }

    protected override void Unapply()
    {
      _protections.RemoveModfier(_modifier);
    }
  }
}