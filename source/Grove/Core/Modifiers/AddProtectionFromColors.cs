namespace Grove.Core.Modifiers
{
  public class AddProtectionFromColors : Modifier
  {
    public ManaColors Colors { get; set; }
    private Protections _protections;

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