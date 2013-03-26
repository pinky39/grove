namespace Grove.Core.Modifiers
{
  using System;
  using Mana;

  public class AddProtectionFromColors : Modifier
  {
    private readonly Func<Modifier, ManaColors> _colors;
    private Protections _protections;

    private AddProtectionFromColors() {}

    public AddProtectionFromColors(Func<Modifier, ManaColors> colors)
    {
      _colors = colors;
    }

    public AddProtectionFromColors(ManaColors colors) : this(delegate { return colors; }) {}

    public override void Apply(Protections protections)
    {
      _protections = protections;
      _protections.AddProtectionFromColors(_colors(this));
    }

    protected override void Unapply()
    {
      _protections.RemoveProtectionFromColors(_colors(this));
    }
  }
}