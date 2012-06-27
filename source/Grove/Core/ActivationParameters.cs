namespace Grove.Core
{
  using Infrastructure;

  [Copyable]
  public class ActivationParameters
  {
    public ITarget CostTarget { get; set; }
    public ITarget Target { get; set; }
    public ITarget DamageSourceTarget { get; set; }

    public static ActivationParameters Default
    {
      get { return new ActivationParameters(); }
    }
    
    public bool PayKicker { get; set; }
    public int? X { get; set; }
  }
}