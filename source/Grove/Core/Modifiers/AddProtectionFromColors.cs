namespace Grove.Core.Modifiers
{
  using Mana;

  public class AddProtectionFromColors : Modifier
  {
    private readonly ManaColors _colors;
    private Protections _protections;

    public AddProtectionFromColors(ManaColors colors)
    {
      _colors = colors;
    }

    public override void Apply(Protections protections)
    {
      _protections = protections;
      _protections.AddProtectionFromColors(_colors);
    }

    protected override void Unapply()
    {
      _protections.RemoveProtectionFromColors(_colors);
    }
  }
}