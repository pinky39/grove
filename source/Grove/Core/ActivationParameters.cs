namespace Grove.Core
{
  using Infrastructure;

  [Copyable]
  public class ActivationParameters
  {
    public ITarget CostTarget { get; set; }

    public static ActivationParameters Default
    {
      get { return new ActivationParameters(); }
    }

    public ITarget EffectTarget { get; set; }
    public bool PayKicker { get; set; }
    public int? X { get; set; }
  }
}