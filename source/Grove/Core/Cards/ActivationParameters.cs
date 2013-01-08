namespace Grove.Core.Cards
{
  using Infrastructure;
  using Targeting;

  [Copyable]
  public class ActivationParameters
  {
    private readonly Targets _targets;

    public ActivationParameters(Targets targets = null, int? x = null)
    {
      _targets = targets ?? new Targets();      
      X = x;
    }

    private ActivationParameters() {}

    public Targets Targets { get { return _targets; } }
    public static ActivationParameters Default { get { return new ActivationParameters(); } }    
    public int? X { get; private set; }
  }
}