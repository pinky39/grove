namespace Grove.Core
{
  using Infrastructure;
  using Targeting;

  [Copyable]
  public class ActivationParameters
  {
    public Targets Targets = new Targets();
    public int? X { get; set; }

    public static ActivationParameters Default { get { return new ActivationParameters(); } }
  }
}