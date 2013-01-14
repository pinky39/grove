namespace Grove.Core.Modifiers
{
  using Grove.Core.Mana;

  public class AddProtectionFromColors : Modifier
  {
    private Protections _protections;
    public ManaColors Colors { get; set; }

    public override void Apply(Protections protections)
    {
      _protections = protections;
      _protections.AddProtectionFromColors(Colors);
    }

    protected override void Unapply()
    {
      _protections.RemoveProtectionFromColors(Colors);
    }
  }
}